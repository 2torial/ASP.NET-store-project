namespace ASP.NET_store_project.Server.Controllers.BasketController;

using Data;
using Data.DataRevised;
using Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Models;
using Models.ComponentData;
using Models.StructuredData;
using System.Linq;
using Utilities;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = IdentityData.RegularUserPolicyName)]
public class BasketController(AppDbContext context, IHttpClientFactory httpClientFactory) : ControllerBase
{
    // Summarizes and places order
    [HttpPost("/api/basket/summary")]
    public async Task<IActionResult> Summary(OrderSummaryData orderData)
    {
        // Validates provided personal information
        var validationResult = new OrderSummaryDataValidator().Validate(orderData);
        if (!validationResult.IsValid) return this.MultipleErrorsBadRequest(validationResult.ToDictionary());

        // Identifies customer
        var jwtToken = new JsonWebToken(Request.Cookies["Token"]);
        var customer = context.Users
            .Include(cust => cust.BasketProducts)
                .ThenInclude(basket => basket.Supplier)
            .SingleOrDefault(customer => customer.Id == Guid.Parse(jwtToken.Subject));
        if (customer == null)
            return this.SingleErrorBadRequest("This customer does not exist!");

        // Identifies selected products
        var orderBases = orderData.Orders
            .Select(order =>
            {
                var orderedProducts = order.ProductBasketIds
                    .Join(customer.BasketProducts,
                        basketId => basketId,
                        basketProd => basketProd.DatabaseId,
                        (_, prod) => prod);
                var firstProduct = orderedProducts.First();

                return orderedProducts.Count() != order.ProductBasketIds.Count() || orderedProducts.Any(prod => prod.SupplierId != firstProduct.SupplierId)
                     ? null : new { firstProduct.Supplier, Products = orderedProducts, DeliveryCost = order.DecimalDeliveryCost() };
            });
        if (orderBases.Any(order => order == null))
            return this.SingleErrorBadRequest("Some of the requested products couldn't be identified.");

        // Collects all selected BasketProduct objects
        var selectedProducts = orderBases
            .SelectMany(order => order!.Products);

        // Requests potential order placement confirmation from related suppliers and transforms recieved data into order objects
        var orders = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
            .PostAsync(orderBases,
                order => new(
                    httpClientFactory.CreateClient(order!.Supplier.Name),
                    order.Supplier.SelectedProductsRequestAdress,
                    JsonContentConverter.Convert(order.Products.Select(prod => new ProductInfo() { Id = prod.ProductId, Quantity = prod.Quantity }))),
                (order, resultProds) => resultProds == null ? null : new
                {
                    order!.Supplier,
                    Info = new OrderInfo(
                        resultProds.Select(prod => prod.NewModified(order!.Supplier)),
                        order!.DeliveryCost,
                        orderData.CustomerDetails,
                        orderData.AdressDetails)
                });
        if (orders.Any(order => order == null))
            return this.SingleErrorBadRequest("System couldn't place new order because of communication error, try again later.");

        // Checks avaliability of requested products
        var confirmedProducts = orders
            .SelectMany(order => order!.Info.Products);
        if (!selectedProducts.All(sProd => confirmedProducts.Any(cProd => sProd.ProductId == cProd.Id && sProd.Quantity == cProd.Quantity)))
            return this.SingleErrorBadRequest("Not all products are avaliable for sell.");

        // Requests order placement from related suppliers
        var orderAcceptResults = await MultipleRequestsEndpoint<string>
            .PostAsync(orders,
                order => new(
                    httpClientFactory.CreateClient(order!.Supplier.Name),
                    $"{order.Supplier.OrderAcceptRequestAdress}/{order.Supplier.StoreExternalId}/{customer.Id}",
                    JsonContentConverter.Convert(order.Info)),
                async msg => msg.IsSuccessStatusCode ? await msg.Content.ReadAsStringAsync() : null,
                (summary, orderId) => new { summary!.Supplier, OrderId = orderId });

        // Identifies succeeded orders
        var succeededOrders = orderAcceptResults
            .Where(result => result?.OrderId != null);

        // If number of succeeded orders is lower than number of placed orders, attempts order cancelation
        if (succeededOrders.Count() < orders.Count())
        {
            var orderCancelResults = await MultipleRequestsEndpoint<bool>
                .PostAsync(succeededOrders,
                    order => new(
                        httpClientFactory.CreateClient(order!.Supplier.Name),
                        order.Supplier.OrderCancelRequestAdress, 
                        JsonContentConverter.Convert(order.OrderId)), 
                    async msg => await Task.FromResult(msg.IsSuccessStatusCode),
                    (order, res) => new { order!.Supplier, IsSucces = res });

            if (!orderCancelResults.All(result => result?.IsSucces ?? false))
            {
                // This is a block for an edge case situation when order was accepted partialy
                // All items should be either accepted or canceled before the order resolves
                // This project doesn't contain an implementation for this scenario
                return this.SingleErrorBadRequest("Critical error occured: Some of the products ordered were issued while the others were not. Contact an administrator.");
            }
            return this.SingleErrorBadRequest("Failed to issue an order.");
        }

        // Removes ordered products from user's basket
        context.RemoveRange(selectedProducts);
        context.SaveChanges();

        return Ok("Order summarized successfuly.");
    }

    // Adds product to the basket
    [HttpGet("/api/basket/add/{supplierId}/{productId}")]
    public async Task<IActionResult> AddItem([FromRoute] Guid supplierId, [FromRoute] string productId)
    {
        // Identifies customer
        var jwtToken = new JsonWebToken(Request.Cookies["Token"]);
        var customer = context.Users
            .Include(user => user.BasketProducts)
            .SingleOrDefault(customer => customer.Id == Guid.Parse(jwtToken.Subject));
        if (customer == null)
            return BadRequest("This customer doesn't exist!");

        // Identifies supplier
        var supplier = context.Suppliers
            .SingleOrDefault(sup => sup.Id == supplierId);
        if (supplier == null)
            return BadRequest("Selected supplier doesn't exist!");

        // Calculates basket quantity
        var selectedProduct = customer.BasketProducts
            .SingleOrDefault(item => item.SupplierId == supplierId && item.ProductId == productId);
        var requestedQuantity = selectedProduct != null ? selectedProduct.Quantity + 1 : 1;

        // Confirms product avaliablity
        var message = await httpClientFactory.CreateClient(supplier.Name)
            .PostAsync(supplier.SelectedProductsRequestAdress,
                JsonContentConverter.Convert(new List<ProductInfo>() { { new ProductInfo() { Id = productId, Quantity = requestedQuantity } } }));
        var isAvaliable = message.IsSuccessStatusCode
            && message.Content.ReadFromJsonAsync<IEnumerable<ProductInfo>>().Result?
                .SingleOrDefault()?.Quantity == requestedQuantity;
        if (!isAvaliable)
            return BadRequest($"Cannot add more products of this type to the basket (unavaliable).");

        // Adds product to the basket (either creates a new record or modifies an existing one)
        if (selectedProduct != null) selectedProduct.Quantity += 1;
        else context.Add(new BasketProduct(productId, customer.Id, supplierId, 1));
        context.SaveChanges();

        return Ok($"Added product {supplierId}:{productId} (user: {customer.UserName}).");
    }

    // Removes product from the basket
    [HttpGet("/api/basket/remove/{supplierId}/{productId}")]
    public IActionResult RemoveItem([FromRoute] Guid supplierId, [FromRoute] string productId)
    {
        // Identifies customer
        var jwtToken = new JsonWebToken(Request.Cookies["Token"]);
        var customer = context.Users
            .Include(user => user.BasketProducts)
            .SingleOrDefault(customer => customer.Id == Guid.Parse(jwtToken.Subject));
        if (customer == null)
            return BadRequest("This customer does not exist!");

        // Removes product from the basket (either removes an existing record or modifies it)
        var selectedProduct = customer.BasketProducts
            .SingleOrDefault(prod => prod.SupplierId == supplierId && prod.ProductId == productId);
        if (selectedProduct != null)
        {
            if (selectedProduct.Quantity > 1)
                selectedProduct.Quantity -= 1;
            else
                context.Remove(selectedProduct);
            context.SaveChanges();
            return Ok($"Removed product {supplierId}:{productId} (user: {customer.UserName}).");
        }

        return BadRequest("There is no such product in the basket!");
    }

    // Collects basket data
    [HttpGet("/api/basket")]
    public async Task<IActionResult> Basket()
    {
        // Identifies user
        var jwtToken = new JsonWebToken(Request.Cookies["Token"]);
        var customer = context.Users
            .Include(cust => cust.BasketProducts)
            .ThenInclude(basket => basket.Supplier)
            .SingleOrDefault(customer => customer.Id == Guid.Parse(jwtToken.Subject));
        if (customer == null)
            return Unauthorized("This customer does not exist!");

        // Groups basket products by their suppliers
        var basket = customer.BasketProducts
            .GroupBy(
                prod => prod.Supplier,
                prod => new ProductInfo() { Id = prod.ProductId, BasketId = prod.DatabaseId.ToString(), Quantity = prod.Quantity },
                (sup, prods) => new { Supplier = sup, Products = prods });

        // If basket is empty, returns an empty component
        if (!basket.Any())
            return Ok(new BasketComponentData([]));

        // Collects product data from related suppliers
        var basketProducts = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
            .PostAsync(basket,
                groupedProds => new(
                    httpClientFactory.CreateClient(groupedProds.Supplier.Name),
                    groupedProds.Supplier.SelectedProductsRequestAdress,
                    JsonContentConverter.Convert(groupedProds.Products)),
                (group, prods) => prods?.Select(prod => prod.NewModified(group.Supplier)))
            .ContinueWith(group => group.Result
                .SelectMany(prods => prods ?? []));

        // Compares basket products to collected product data
        var innerResults = customer.BasketProducts 
            .Join(basketProducts,
                serverSideProd => serverSideProd.ProductId,
                responseSideProd => responseSideProd.Id,
                (ssp, rsp) => new { ssp.DatabaseId, IsFullyAvaliable = ssp.Quantity == rsp.Quantity, AvaliableQuantity = rsp.Quantity });

        // Aborts if not all products from the basket were confirmed by the supplier
        if (innerResults.Count() < customer.BasketProducts.Count)
            return BadRequest("Products in your basket are temporarily unavaliable.");

        // Updates basket products that are not avaliable for sell in requested amounts (either removes an existing record or modifies it)
        var toUpdate = innerResults.Where(res => !res.IsFullyAvaliable);
        if (toUpdate.Any())
        {
            var productsToModify = customer.BasketProducts
            .Join(toUpdate,
                basketProd => basketProd.DatabaseId,
                prodInfo => prodInfo.DatabaseId,
                (basketProd, prodInfo) => (basketProd, prodInfo.AvaliableQuantity));
            foreach (var (prod, quantity) in productsToModify)
            {
                if (quantity > 0)
                    prod.Quantity = quantity;
                else
                    context.Remove(prod);
            }
            context.SaveChanges();
        }

        return Ok(new BasketComponentData(basketProducts));
    }
}
