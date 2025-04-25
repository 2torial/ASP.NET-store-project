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
                .Where(customer => customer.UserName == jwtToken.Subject);
            if (!customer.Any())
                return BadRequest("Customer is missing from the database!");

            var basket = customer
                .SelectMany(cust => cust.BasketProducts)
                .Include(prod => prod.Supplier)
                .AsEnumerable();

            if (!basket.Any())
                return BadRequest("Basket is empty.");

            var orders = basket
                .GroupBy(
                    prod => prod.Supplier,
                    prod => new ProductInfo() { Id = prod.ProductId, Quantity = prod.Quantity },
                    (sup, prods) => new { Supplier = sup, Products = prods });

            var summaryRequestClientsData = orders
                .ToDictionary(
                    order => order.Supplier.Id,
                    order => new ClientData(httpClientFactory.CreateClient(order.Supplier.Name))
                    {
                        Content = JsonContentConverter.Convert(order.Products),
                        RequestAdress = order.Supplier.OrderSummaryRequestAdress,
                    });

            var summaryStatusCodes = await MultipleRequestsEndpoint
                .GetAsync(summaryRequestClientsData, async msg => await Task.FromResult(msg.IsSuccessStatusCode));
            var failedSummaryStatusCodes = summaryStatusCodes
                .Where(kvp => !kvp.Value);
            if (failedSummaryStatusCodes.Any())
                return BadRequest("Failed to issue an order.");

            var orderAcceptRequestClientsData = orders
                .ToDictionary(
                    order => order.Supplier.Id,
                    order => new ClientData(httpClientFactory.CreateClient(order.Supplier.Name))
                    {
                        Content = JsonContentConverter.Convert(order.Products),
                        RequestAdress = order.Supplier.OrderAcceptRequestAdress,
                    });

            var orderAcceptStatusCodes = await MultipleRequestsEndpoint
                .GetAsync(orderAcceptRequestClientsData, async msg => await Task.FromResult(msg.IsSuccessStatusCode));
            var failedOrderAcceptStatusCodes = orderAcceptStatusCodes
                .Where(kvp => !kvp.Value);
            if (failedOrderAcceptStatusCodes.Any())
            {
                var orderCancelRequestClientsData = orders
                    .Where(order => failedOrderAcceptStatusCodes
                        .Select(kvp => kvp.Key)
                        .Contains(order.Supplier.Id))
                    .ToDictionary(
                        order => order.Supplier.Id,
                        order => new ClientData(httpClientFactory.CreateClient(order.Supplier.Name))
                        {
                            Content = JsonContentConverter.Convert(order.Products),
                            RequestAdress = order.Supplier.OrderCancelRequestAdress,
                        });
                var orderCancelStatusCodes = await MultipleRequestsEndpoint
                    .GetAsync(orderCancelRequestClientsData, async msg => await Task.FromResult(msg.IsSuccessStatusCode));
                var failedOrderCancelStatusCodes = orderAcceptStatusCodes
                .Where(kvp => !kvp.Value);
                if (failedOrderCancelStatusCodes.Any())
                {
                    // This is a space for an edge case situation when only some items of an order were accepted
                    // while the other were canceled (all items should be accepted or canceled before an order resolves)
                    return BadRequest("Critical error occured: Some of the products ordered were issued while the others were not. Contact an administrator.");
                }
                return BadRequest("Failed to issue an order.");
            }

            var orderedProducts = orders
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
                return BadRequest("This customer does not exist!");

            var groupedBasketProducts = customer.BasketProducts
                .GroupBy(
                    prod => prod.Supplier,
                    prod => new ProductInfo() { Id = prod.ProductId, Quantity = prod.Quantity },
                    (sup, prods) => new { Supplier = sup, Products = prods });

            var basketProductsRequestClientsData = groupedBasketProducts
                .ToDictionary(
                    basket => basket.Supplier,
                    basket => new ClientData(httpClientFactory.CreateClient(basket.Supplier.Name))
                    {
                        Content = JsonContentConverter.Convert(basket.Products),
                        RequestAdress = basket.Supplier.SelectedProductsRequestAdress,
                    });

            var basketProductsBatch = await MultipleRequestsEndpoint
                .SendAsync<Supplier, IEnumerable<ProductInfo>>(basketProductsRequestClientsData);

            var basketProducts = basketProductsBatch
                .SelectMany(
                    batch => batch.Value ?? [],
                    (batch, prod) => prod.Modify(batch.Key));

            var innerResults = customer.BasketProducts // Checks if all requested products are recieved in appropriate quantities
                .Join(basketProducts,
                    serverSideProd => serverSideProd.ProductId,
                    requestSideProd => requestSideProd.Id,
                    (ssp, rsp) => new { ssp.ProductId, ssp.SupplierId, QuantityDifference = ssp.Quantity - rsp.Quantity });
            if (innerResults.Count() < customer.BasketProducts.Count || innerResults.Any(res => res.QuantityDifference != 0))
                return BadRequest(innerResults);

            var data = new BasketComponentData
            {
                Products = basketProducts,
            };

            return Ok(data);
        }
    }
}
