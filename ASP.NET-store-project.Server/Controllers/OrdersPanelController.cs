using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using ASP.NET_store_project.Server.Models.ComponentData;
using ASP.NET_store_project.Server.Utilities.MultipleRequests;
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
        //[HttpGet("/api/orders")]
        //public async Task<IActionResult> GetOrders()
        //{
        //    var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
        //    var customer = context.Users
        //        .SingleOrDefault(customer => customer.UserName == jwtToken.Subject);
        //    if (customer == null)
        //        return BadRequest("Customer is missing from the database!");

        //    var suppliers = context.Suppliers
        //        .Select(sup => new { Data = sup, Client = httpClientFactory.CreateClient(sup.Name) })
        //        .AsEnumerable();

        //    var orderListRequestClientsData = suppliers
        //        .ToDictionary(
        //            sup => sup.Data,
        //            sup => new ClientData(sup.Client)
        //            {
        //                RequestAdress = $"{sup.Data.OrderListRequestAdress}/{customer.Id}",
        //            });

        //    var orderListBatch = await MultipleRequestsEndpoint<int>
        //       .SendAsync(orderListRequestClientsData, async msg => await Task.FromResult(1));
        //    var orderList = orderListBatch
        //        .SelectMany(
        //            kvp => kvp.Value ?? [],
        //            (batch, prod) => prod.Modify(batch.Key));

        //    return Ok(new OrderListComponentData()
        //    {
        //        Orders = context.Orders
        //            .Select(order => new OrderListComponentData.OrderData
        //            {
        //                OrderId = order.Id,
        //                CustomerDetails = new OrderListComponentData.OrderData.UserData
        //                {
        //                    CustomerId = order.Customer.Id,
        //                    UserName = order.Customer.UserName,
        //                    Name = order.Customer.CustomerDetails.Name,
        //                    Surname = order.Customer.CustomerDetails.Surname,
        //                    PhoneNumber = order.Customer.CustomerDetails.PhoneNumber,
        //                    Email = order.Customer.CustomerDetails.Email,
        //                },
        //                AdressDetails = new OrderListComponentData.OrderData.AdressData
        //                {
        //                    Region = order.Customer.AdressDetails.Region,
        //                    City = order.Customer.AdressDetails.City,
        //                    PostalCode = order.Customer.AdressDetails.PostalCode,
        //                    StreetName = order.Customer.AdressDetails.StreetName,
        //                    HouseNumber = order.Customer.AdressDetails.HouseNumber,
        //                    ApartmentNumber = order.Customer.AdressDetails.ApartmentNumber,
        //                },
        //                CurrentStatus = null,
        //            }).ToList(),
        //    });
        //}
    }
}
