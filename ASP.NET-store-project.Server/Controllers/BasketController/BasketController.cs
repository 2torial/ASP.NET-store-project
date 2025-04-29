using ASP.NET_store_project.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using ASP.NET_store_project.Server.Models;
using Microsoft.EntityFrameworkCore;
using ASP.NET_store_project.Server.Utilities.MultipleRequests;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Models.ComponentData.BasketComponentData;
using ASP.NET_store_project.Server.Models.StructuredData;
using ASP.NET_store_project.Server.Utilities;

namespace ASP.NET_store_project.Server.Controllers.BasketController
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IdentityData.RegularUserPolicyName)]
    public class BasketController(AppDbContext context, IHttpClientFactory httpClientFactory) : ControllerBase
    {
        [HttpPost("/api/basket/summary")]
        public async Task<IActionResult> Summary([FromForm] OrderSummaryData orderData)
        {
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Include(cust => cust.BasketProducts)
                .ThenInclude(basket => basket.Supplier)
                .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
            if (customer == null)
                return BadRequest("This customer does not exist!");

            var basket = customer.BasketProducts
                .GroupBy(
                    prod => prod.Supplier,
                    prod => new ProductInfo() { Id = prod.ProductId, Quantity = prod.Quantity },
                    (sup, prods) => new { Supplier = sup, Products = prods });

            if (!basket.Any())
                return BadRequest("Basket is empty.");

            var summaryResults = await MultipleRequestsEndpoint<bool>
                .GetAsync(basket,
                    groupedProds => new(
                        httpClientFactory.CreateClient(groupedProds.Supplier.Name), 
                        groupedProds.Supplier.OrderSummaryRequestAdress),
                    async msg => await Task.FromResult(msg.IsSuccessStatusCode),
                    (groupedProds, res) => new { groupedProds.Supplier, IsSucces = res });

            if (summaryResults.All(result => result?.IsSucces ?? false))
                return BadRequest("Failed to issue an order.");

            var orderAcceptResults = await MultipleRequestsEndpoint<bool>
                .PostAsync(basket,
                    groupedProds => new(
                        httpClientFactory.CreateClient(groupedProds.Supplier.Name),
                        groupedProds.Supplier.OrderAcceptRequestAdress,
                        JsonContentConverter.Convert(groupedProds.Products)),
                    async msg => await Task.FromResult(msg.IsSuccessStatusCode),
                    (groupedProds, res) => new { groupedProds.Supplier, IsSucces = res });

            var succeededSupplierIds = orderAcceptResults
                .Where(result => result?.IsSucces ?? false)
                .Select(result => result!.Supplier.Id);

            var failedOrders = basket
                .Where(groupedProds => !succeededSupplierIds.Contains(groupedProds.Supplier.Id));

            if (failedOrders.Any())
            {
                var orderCancelResults = await MultipleRequestsEndpoint<bool>
                    .PostAsync(failedOrders,
                        groupedProds => new(
                            httpClientFactory.CreateClient(groupedProds.Supplier.Name), 
                            groupedProds.Supplier.OrderCancelRequestAdress, 
                            JsonContentConverter.Convert(groupedProds.Products)), 
                        async msg => await Task.FromResult(msg.IsSuccessStatusCode),
                        (groupedProds, res) => new { groupedProds.Supplier, IsSucces = res });

                if (!orderCancelResults.All(result => result?.IsSucces ?? false))
                {
                    // This is a space for an edge case situation where only some part of an order was accepted
                    // while the other was canceled (all items should be accepted or canceled before the order resolves)
                    return BadRequest("Critical error occured: Some of the products ordered were issued while the others were not. Contact an administrator.");
                }
                return BadRequest("Failed to issue an order.");
            }

            var orderedProducts = basket
                .SelectMany(order => order.Products);
            context.BasketProducts
                .RemoveRange(context.BasketProducts
                    .Where(basketProd => orderedProducts
                        .Select(orderedProd => orderedProd.Id)
                        .Contains(basketProd.ProductId)));
            context.SaveChanges();

            return Ok("Order summarized successfuly.");
        }

        [HttpGet("/api/basket/add/{supplierId}/{productId}")]
        public IActionResult AddItem([FromRoute] Guid supplierId, [FromRoute] string productId)
        {
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Include(user => user.BasketProducts)
                .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
            if (customer == null)
                return BadRequest("This customer does not exist!");

            var selectedProduct = customer.BasketProducts
                .SingleOrDefault(item => item.SupplierId == supplierId && item.ProductId == productId);

            if (selectedProduct != null)
                selectedProduct.Quantity += 1;
            else
                context.BasketProducts.Add(
                    new BasketProduct(productId, customer.Id, supplierId, 1));
            context.SaveChanges();

            return Ok($"Added product {supplierId}:{productId} (user: {customer.UserName}).");
        }

        [HttpGet("/api/basket/remove/{supplierId}/{productId}")]
        public IActionResult RemoveItem([FromRoute] Guid supplierId, [FromRoute] string productId)
        {
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Include(user => user.BasketProducts)
                .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
            if (customer == null)
                return BadRequest("This customer does not exist!");

            var selectedProduct = customer.BasketProducts
                .SingleOrDefault(prod => prod.SupplierId == supplierId && prod.ProductId == productId);

            if (selectedProduct != null)
            {
                selectedProduct.Quantity -= 1;
                if (selectedProduct.Quantity == 0)
                    context.BasketProducts.Remove(selectedProduct);
                context.SaveChanges();
            }
            
            return Ok($"Removed product {supplierId}:{productId} (user: {customer.UserName}).");
        }

        [HttpGet("/api/basket")]
        public async Task<IActionResult> Basket()
        {
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Include(cust => cust.BasketProducts)
                .ThenInclude(basket => basket.Supplier)
                .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
            if (customer == null)
                return Unauthorized("This customer does not exist!");

            var basket = customer.BasketProducts
                .GroupBy(
                    prod => prod.Supplier,
                    prod => new ProductInfo() { Id = prod.ProductId, Quantity = prod.Quantity },
                    (sup, prods) => new { Supplier = sup, Products = prods });

            if (!basket.Any())
                return Ok(new BasketComponentData() { Products = [] });

            var basketProductsBatch = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .PostAsync(basket,
                    groupedProds => new(
                        httpClientFactory.CreateClient(groupedProds.Supplier.Name),
                        groupedProds.Supplier.SelectedProductsRequestAdress,
                        JsonContentConverter.Convert(groupedProds.Products)),
                    (group, prods) => prods?.Select(prod => new ProductInfo(prod).Modify(group.Supplier)));

            var basketProducts = basketProductsBatch
                .SelectMany(batch => batch ?? []);

            var innerResults = customer.BasketProducts // Checks if all requested products are recieved in appropriate quantities
                .Join(basketProducts,
                    serverSideProd => serverSideProd.ProductId,
                    requestSideProd => requestSideProd.Id,
                    (ssp, rsp) => new { ssp.ProductId, ssp.SupplierId, QuantityDifference = ssp.Quantity - rsp.Quantity });
            if (innerResults.Count() < customer.BasketProducts.Count() || innerResults.Any(res => res.QuantityDifference != 0))
                return BadRequest(innerResults);

            return Ok(new BasketComponentData() { Products = basketProducts });
        }
    }
}
