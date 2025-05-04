using ASP.NET_store_project.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using ASP.NET_store_project.Server.Models;
using Microsoft.EntityFrameworkCore;
using ASP.NET_store_project.Server.Utilities.MultipleRequests;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Models.StructuredData;
using ASP.NET_store_project.Server.Utilities;
using ASP.NET_store_project.Server.Models.ComponentData;

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

            var orderAcceptResults = await MultipleRequestsEndpoint<string>
                .PostAsync(basket,
                    groupedProds => new(
                        httpClientFactory.CreateClient(groupedProds.Supplier.Name),
                        $"{groupedProds.Supplier.OrderAcceptRequestAdress}/[0]/{customer.Id}",
                        JsonContentConverter.Convert(new OrderInfo(
                            groupedProds.Products, 
                            new(orderData.Name, orderData.Surname, orderData.PhoneNumber, orderData.Email),
                            new(orderData.Region, orderData.City, orderData.PostalCode, orderData.StreetName, orderData.HouseNumber, orderData.ApartmentNumber)))),
                    async msg => msg.IsSuccessStatusCode ? await msg.Content.ReadAsStringAsync() : null,
                    (groupedProds, orderId) => new { groupedProds.Supplier, OrderId = orderId });

            var succeededOrders = orderAcceptResults
                .Where(result => result?.OrderId != null);

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
                    // This is a space for an edge case situation where only some part of an order was accepted
                    // while the other was canceled (all items should be accepted or canceled before the order resolves)
                    return BadRequest("Critical error occured: Some of the products ordered were issued while the others were not. Contact an administrator.");
                }
                return BadRequest("Failed to issue an order.");
            }

            context.BasketProducts
                .RemoveRange(customer.BasketProducts);

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

            var basketProducts = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .PostAsync(basket,
                    groupedProds => new(
                        httpClientFactory.CreateClient(groupedProds.Supplier.Name),
                        groupedProds.Supplier.SelectedProductsRequestAdress,
                        JsonContentConverter.Convert(groupedProds.Products)),
                    (group, prods) => prods?.Select(prod => new ProductInfo(prod).Modify(group.Supplier)))
                .ContinueWith(group => group.Result
                    .SelectMany(prods => prods ?? []));

            var innerResults = customer.BasketProducts // Checks if all requested products are recieved in appropriate quantities
                .Join(basketProducts,
                    serverSideProd => serverSideProd.ProductId,
                    requestSideProd => requestSideProd.Id,
                    (ssp, rsp) => new { ssp.ProductId, ssp.SupplierId, QuantityDifference = ssp.Quantity - rsp.Quantity });
            if (innerResults.Count() < customer.BasketProducts.Count || innerResults.Any(res => res.QuantityDifference != 0))
                return BadRequest(innerResults);

            return Ok(new BasketComponentData { Products = basketProducts });
        }
    }
}
