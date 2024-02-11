using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ASP.NET_store_project.Server.Models.UserListComponentData;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    public class AdminPanelController(AppDbContext context) : ControllerBase
    {
        [HttpGet("/api/admin/users")]
        public IActionResult GetUsers()
        {
            return Ok(new UserListComponentData()
            {
                Users = context.Users
                    .Select(user => new UserData
                    {
                        Name = user.UserName,
                        IsAdmin = user.IsAdmin,
                    }).ToList()
            });
        }

        [HttpGet("/api/admin/orders")]
        public IActionResult GetOrders()
        {
            return Ok(new OrderListComponentData()
            {
                Orders = context.Orders
                    .Select(order => new OrderListComponentData.OrderData
                    {
                        OrderId = order.OrderId,
                        CustomerDetails = new OrderListComponentData.OrderData.UserData
                        {
                            CustomerId = order.CustomerId,
                            Name = order.CustomerDetails.Name,
                            Surname = order.CustomerDetails.Surname,
                            PhoneNumber = order.CustomerDetails.PhoneNumber,
                            Email = order.CustomerDetails.Email,
                        },
                        AdressDetails = new OrderListComponentData.OrderData.AdressData
                        {
                            Region = order.AdressDetails.Region,
                            City = order.AdressDetails.City,
                            PostalCode = order.AdressDetails.PostalCode,
                            StreetName = order.AdressDetails.StreetName,
                            HouseNumber = order.AdressDetails.HouseNumber,
                            ApartmentNumber = order.AdressDetails.ApartmentNumber,
                        },
                        CurrentStatus = order.StatusChangeHistory
                            .OrderBy(status => status.DateOfChange)
                            .Last().StatusCode
                    }).ToList(),
            });
        }
    }
}
