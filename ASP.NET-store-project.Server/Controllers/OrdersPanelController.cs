namespace ASP.NET_store_project.Server.Controllers;

using Data;
using Models;
using Models.ComponentData;
using Models.StructuredData;
using Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = IdentityData.RegularUserPolicyName)]
public class OrdersPanelController(AppDbContext context, IHttpClientFactory httpClientFactory) : ControllerBase
{
    // Collects past orders from suppliers and returns them to the client
    [HttpGet("/api/orders")]
    public async Task<IActionResult> GetOrders()
    {
        // Identifies user
        var jwtToken = new JsonWebToken(Request.Cookies["Token"]);
        var customer = context.Users
            .SingleOrDefault(customer => customer.Id == Guid.Parse(jwtToken.Subject));
        if (customer == null)
            return BadRequest("Customer is missing from the database!");

        // Collects supplier data
        var suppliers = context.Suppliers.AsEnumerable();

        // Requests past orders from suppliers and populates data missing from recieved OrderInfos
        var orderList = await MultipleRequestsEndpoint<IEnumerable<OrderInfo>>
            .GetAsync(suppliers,
                sup => new(httpClientFactory.CreateClient(sup.Name), $"{sup.OrderListRequestAdress}/{sup.StoreExternalId}/{customer.Id}"),
                (sup, orders) => orders?.Select(order => new OrderInfo(
                    order.Id, 
                    sup.Id.ToString(), 
                    sup.Name,
                    order.Products.Aggregate(0m, (acc, prod) => acc + prod.Price * prod.Quantity), // Sums ordered products' prices
                    order.DeliveryCost,
                    order.DeliveryMethod,
                    order.Products.Select(prod => prod.NewModified(sup, adjustPrice: false)), // Doesn't adjust because suppliers store past store prices
                    order.CustomerDetails, 
                    order.AdressDetails, 
                    order.StageHistory)))
            .ContinueWith(ordersBatch => ordersBatch.Result
                .SelectMany(orders => orders ?? []));

        // Returns component data
        return Ok(new OrderListComponentData(orderList));
    }
}
