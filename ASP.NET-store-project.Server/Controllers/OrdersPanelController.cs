using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using ASP.NET_store_project.Server.Models.ComponentData;
using ASP.NET_store_project.Server.Models.StructuredData;
using ASP.NET_store_project.Server.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IdentityData.RegularUserPolicyName)]
    public class OrdersPanelController(AppDbContext context, IHttpClientFactory httpClientFactory) : ControllerBase
    {
        [HttpGet("/api/orders")]
        public async Task<IActionResult> GetOrders()
        {
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
            if (customer == null)
                return BadRequest("Customer is missing from the database!");

            var suppliers = context.Suppliers.AsEnumerable();

            var orderList = await MultipleRequestsEndpoint<IEnumerable<OrderInfo>>
                .GetAsync(suppliers,
                    sup => new(httpClientFactory.CreateClient(sup.Name), $"{sup.OrderListRequestAdress}/[0]/{customer.Id}"),
                    (sup, orders) => orders?.Select(order => new OrderInfo(
                        order.Id, 
                        sup.Id.ToString(), 
                        sup.Name,
                        order.Products.Select(prod => prod.NewModified(sup)),
                        order.CustomerDetails, 
                        order.AdressDetails, 
                        order.Stage)))
                .ContinueWith(ordersBatch => ordersBatch.Result
                    .SelectMany(orders => orders ?? []));

            return Ok(new OrderListComponentData(orderList));
        }
    }
}
