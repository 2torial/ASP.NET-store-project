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
                        OrderId = order.Id,
                        CustomerDetails = new OrderListComponentData.OrderData.UserData
                        {
                            CustomerId = order.CustomerId,
                            Name = order.Customer.CustomerDetails.Name,
                            Surname = order.Customer.CustomerDetails.Surname,
                            PhoneNumber = order.Customer.CustomerDetails.PhoneNumber,
                            Email = order.Customer.CustomerDetails.Email,
                        },
                        AdressDetails = new OrderListComponentData.OrderData.AdressData
                        {
                            Region = order.Customer.AdressDetails.Region,
                            City = order.Customer.AdressDetails.City,
                            PostalCode = order.Customer.AdressDetails.PostalCode,
                            StreetName = order.Customer.AdressDetails.StreetName,
                            HouseNumber = order.Customer.AdressDetails.HouseNumber,
                            ApartmentNumber = order.Customer.AdressDetails.ApartmentNumber,
                        },
                        CurrentStatus = null,
                    }).ToList(),
            });
        }
        [HttpGet("/api/admin/items")]
        public IActionResult GetItems()
        {
            return Ok(new ItemListComponentData()
            {
                Items = context.Items
                    .Select(item => new ItemListComponentData.ItemData
                    {
                        Item = new StoreItems.Item
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Price = item.Price,
                            Gallery = item.Gallery
                            .Select(image => image.Content)
                            .ToList(),
                            Specification = item.Configurations
                            .Select(config => new StoreItems.Item.Configuration
                            {
                                Label = config.Label,
                                Parameter = config.Parameter,
                            })
                            .ToList(),
                            PageLink = item.Page,
                        },
                        IsDeleted = item.IsDeleted,
                    }).ToList(),
            });
        }

        [HttpGet("/api/admin/items/set/unavaliable/{itemId}")]
        public IActionResult SetUnavaliable([FromRoute] int itemId)
        {
            var item = context.Items.Where(item => item.Id == itemId);
            if (!item.Any())
                return BadRequest("This item does not exists.");
            item.Single().IsDeleted = true;
            context.SaveChanges();
            return Ok("Item " + item.Single().Name + " is now set as unavaliable.");
        }

        [HttpGet("/api/admin/items/set/avaliable/{itemId}")]
        public IActionResult SetAvaliable([FromRoute] int itemId)
        {
            var item = context.Items.Where(item => item.Id == itemId);
            if (!item.Any())
                return BadRequest("This item does not exists.");
            item.Single().IsDeleted = false;
            context.SaveChanges();
            return Ok("Item " + item.Single().Name + " is now set as avaliable.");
        }
    }
}
