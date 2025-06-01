using ASP.NET_store_project.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using ASP.NET_store_project.Server.Models;
using Microsoft.EntityFrameworkCore;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Models.StructuredData;
using ASP.NET_store_project.Server.Utilities;
using ASP.NET_store_project.Server.Models.ComponentData;
using ASP.NET_store_project.Server.Extentions;

namespace ASP.NET_store_project.Server.Controllers.BasketController
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IdentityData.RegularUserPolicyName)]
    public class BasketController(AppDbContext context, IHttpClientFactory httpClientFactory) : ControllerBase
    {
        // Summarizes new order based on products selected from the basket
        [HttpPost("/api/basket/summary")]
        public async Task<IActionResult> Summary(OrderSummaryData orderData)
        {
            // Validates provided personal information
            var validationResult = new OrderSummaryDataValidator().Validate(orderData);
            if (!validationResult.IsValid) return this.MultipleErrorsBadRequest(validationResult.ToDictionary());

            // Identifies the customer
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Include(cust => cust.BasketProducts)
                    .ThenInclude(basket => basket.Supplier)
                .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
            if (customer == null)
                return this.SingleErrorBadRequest("This customer does not exist!");

            // Filters selected products from all of the products stored in the database basket
            var selectedBasketIds = orderData.Orders
                .SelectMany(order => order.ProductBasketIds);
            var selectedProducts = customer.BasketProducts
                .Join(selectedBasketIds,
                    basketProd => basketProd.DatabaseId,
                    basketId => basketId,
                    (prod, _) => prod);

            // If number of selected products doesn't match the number of their database counterparts - abort
            if (selectedBasketIds.Count() != selectedProducts.Count())
                return this.SingleErrorBadRequest("Unidentified products were found in order.");

            // Groupes products by their suppliers (if there are no products selected - abort)
            var basket = selectedProducts
                .GroupBy(
                    prod => prod.Supplier,
                    prod => new ProductInfo() { Id = prod.ProductId, Quantity = prod.Quantity },
                    (sup, prods) => new { Supplier = sup, Products = prods });
            if (!basket.Any())
                return this.SingleErrorBadRequest("Basket is empty.");

            // Requests potential order placement confirmation from related suppliers
            var confirmedProductsBatch = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .PostAsync(basket,
                    groupedProds => new(
                        httpClientFactory.CreateClient(groupedProds.Supplier.Name),
                        groupedProds.Supplier.SelectedProductsRequestAdress,
                        JsonContentConverter.Convert(groupedProds.Products)),
                    (groupedProds, resultProds) => new { groupedProds.Supplier, Products = resultProds?.Select(prod => prod.NewModified(groupedProds.Supplier)) });

            // Checks if all products are avaliable, otherwise - abort
            var confirmedProducts = confirmedProductsBatch
                .SelectMany(group => group?.Products ?? []);
            if (!selectedProducts.All(sProd => confirmedProducts.Any(cProd => sProd.ProductId == cProd.Id && sProd.Quantity == cProd.Quantity)))
                return this.SingleErrorBadRequest("Not all products are avaliable for sell.");

            // Requests order placement from related suppliers
            var orderAcceptResults = await MultipleRequestsEndpoint<string>
                .PostAsync(confirmedProductsBatch,
                    groupedProds => new(
                        httpClientFactory.CreateClient(groupedProds!.Supplier.Name),
                        $"{groupedProds.Supplier.OrderAcceptRequestAdress}/{groupedProds.Supplier.StoreExternalId}/{customer.Id}",
                        JsonContentConverter.Convert(new OrderInfo(
                            groupedProds.Products!, 0,
                            orderData.CustomerDetails,
                            orderData.AdressDetails))),
                    async msg => msg.IsSuccessStatusCode ? await msg.Content.ReadAsStringAsync() : null,
                    (summary, orderId) => new { summary!.Supplier, OrderId = orderId });

            // Identifies succeeded orders
            var succeededOrders = orderAcceptResults
                .Where(result => result?.OrderId != null);

            // If number of succeeded orders is lower than placed orders, attempt orders' cancelation
            if (succeededOrders.Count() < basket.Count())
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
                    // This is a block for an edge case situation where only some part of an order was accepted
                    // While the other was canceled (all items should be either accepted or canceled before the order resolves)
                    // It is not implemented in this project
                    return this.SingleErrorBadRequest("Critical error occured: Some of the products ordered were issued while the others were not. Contact an administrator.");
                }
                return this.SingleErrorBadRequest("Failed to issue an order.");
            }

            // Removes ordered products from basket
            context.RemoveRange(selectedProducts);
            context.SaveChanges();

            return Ok("Order summarized successfuly.");
        }

        // Adds product to the basket
        [HttpGet("/api/basket/add/{supplierId}/{productId}")]
        public IActionResult AddItem([FromRoute] Guid supplierId, [FromRoute] string productId)
        {
            // Identifies the customer
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Include(user => user.BasketProducts)
                .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
            if (customer == null)
                return BadRequest("This customer does not exist!");

            // Adds product to the basket (either creates new record or modifies it)
            var selectedProduct = customer.BasketProducts
                .SingleOrDefault(item => item.SupplierId == supplierId && item.ProductId == productId);
            if (selectedProduct != null) selectedProduct.Quantity += 1;
            else context.Add(new BasketProduct(productId, customer.Id, supplierId, 1));
            context.SaveChanges();

            return Ok($"Added product {supplierId}:{productId} (user: {customer.UserName}).");
        }

        // Removes product from the basket
        [HttpGet("/api/basket/remove/{supplierId}/{productId}")]
        public IActionResult RemoveItem([FromRoute] Guid supplierId, [FromRoute] string productId)
        {
            // Identifies the customer
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Include(user => user.BasketProducts)
                .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
            if (customer == null)
                return BadRequest("This customer does not exist!");

            // Removes product from the basket (either removes existing record or modifies it)
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

        // Returns basket data
        [HttpGet("/api/basket")]
        public async Task<IActionResult> Basket()
        {
            // Identifies the user
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Include(cust => cust.BasketProducts)
                .ThenInclude(basket => basket.Supplier)
                .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
            if (customer == null)
                return Unauthorized("This customer does not exist!");

            // Groupes basket products by their suppliers
            var basket = customer.BasketProducts
                .GroupBy(
                    prod => prod.Supplier,
                    prod => new ProductInfo() { Id = prod.ProductId, BasketId = prod.DatabaseId.ToString(), Quantity = prod.Quantity },
                    (sup, prods) => new { Supplier = sup, Products = prods });

            // If basket is empty, returns an empty component
            if (!basket.Any())
                return Ok(new BasketComponentData([]));

            // Otherwise requests product information from related suppliers
            var basketProducts = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .PostAsync(basket,
                    groupedProds => new(
                        httpClientFactory.CreateClient(groupedProds.Supplier.Name),
                        groupedProds.Supplier.SelectedProductsRequestAdress,
                        JsonContentConverter.Convert(groupedProds.Products)),
                    (group, prods) => prods?.Select(prod => prod.NewModified(group.Supplier)))
                .ContinueWith(group => group.Result
                    .SelectMany(prods => prods ?? []));

            // Checks if all requested products are recieved in appropriate quantities
            var innerResults = customer.BasketProducts 
                .Join(basketProducts,
                    serverSideProd => serverSideProd.ProductId,
                    requestSideProd => requestSideProd.Id,
                    (ssp, rsp) => new { ssp.ProductId, ssp.SupplierId, QuantityDifference = ssp.Quantity - rsp.Quantity });
            if (innerResults.Count() < customer.BasketProducts.Count || innerResults.Any(res => res.QuantityDifference != 0))
                return BadRequest(innerResults);

            return Ok(new BasketComponentData(basketProducts));
        }
    }
}
