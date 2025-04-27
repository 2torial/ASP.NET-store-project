using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using ASP.NET_store_project.Server.Models.ComponentData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IdentityData.RegularUserPolicyName)]
    public class OrdersPanelController(AppDbContext context) : ControllerBase
    {
        [HttpGet("/api/admin/orders")]
        public IActionResult GetOrders()
        {
            return Ok(new OrderListComponentData()
            {
                //Orders = context.Orders
                //    .Select(order => new OrderListComponentData.OrderData
                //    {
                //        OrderId = order.Id,
                //        CustomerDetails = new OrderListComponentData.OrderData.UserData
                //        {
                //            CustomerId = order.Customer.Id,
                //            UserName = order.Customer.UserName,
                //            Name = order.Customer.CustomerDetails.Name,
                //            Surname = order.Customer.CustomerDetails.Surname,
                //            PhoneNumber = order.Customer.CustomerDetails.PhoneNumber,
                //            Email = order.Customer.CustomerDetails.Email,
                //        },
                //        AdressDetails = new OrderListComponentData.OrderData.AdressData
                //        {
                //            Region = order.Customer.AdressDetails.Region,
                //            City = order.Customer.AdressDetails.City,
                //            PostalCode = order.Customer.AdressDetails.PostalCode,
                //            StreetName = order.Customer.AdressDetails.StreetName,
                //            HouseNumber = order.Customer.AdressDetails.HouseNumber,
                //            ApartmentNumber = order.Customer.AdressDetails.ApartmentNumber,
                //        },
                //        CurrentStatus = null,
                //    }).ToList(),
            });
        }
    }
}
