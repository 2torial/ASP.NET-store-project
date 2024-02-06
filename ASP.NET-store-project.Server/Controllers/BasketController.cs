using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System.Xml.Linq;
using Microsoft.Extensions.Primitives;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController(
        AppDbContext context, 
        ILogger<StoreController> logger
    ) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        private readonly ILogger<StoreController> _logger = logger;

        [HttpGet("/api/basket")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleType.User))]
        public BasketComponentData Basket()
        {
            logger.LogError("me");
            var basket = context.Customers
                .Where(customer => customer.UserName == "user") // Temporarly without authentication
                .SelectMany(customer => customer.SelectedItems);

            foreach (var item in basket)
                logger.LogError(item.OrderId.ToString());

            basket = basket
                .Where(item => item.OrderId == null);

            return new BasketComponentData
            {
                Items = basket
                    .Select(selectedItem => new BasketComponentData.BasketedItem
                    {
                        Id = selectedItem.Item.Id, // SelectedItem.Id =/= Item.Id

                        Quantity = selectedItem.Quantity,

                        Name = selectedItem.Item.Name,

                        Price = selectedItem.Item.Price,

                        Gallery = selectedItem.Item.Gallery
                            .Select(image => image.Content)
                            .ToList(),

                        PageLink = selectedItem.Item.Page,
                    }).ToList(),
            };
        }

        [HttpPost("/api/basket/summary")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleType.User))]
        public IActionResult Summary()
        {
            var customer = _context.Customers
                .Where(customer => customer.UserName == "user"); // ommit authentication for now
            var basket = customer
                .SelectMany(customer => customer.Basket)
                .Where(item => item.Order == null);

            if (!basket.Any()) return BadRequest("Basket is empty");

            int orderId = _context.Orders.Max(order => order.OrderId) + 1;
            var order = new Order(orderId, customer.Single().UserName);

            if (!Request.Form.TryGetValue("Region", out var region))
                return BadRequest("Region is missing!");
            if (!Request.Form.TryGetValue("City", out var city))
                return BadRequest("City is missing!");
            if (!Request.Form.TryGetValue("PostalCode", out var postalCode))
                return BadRequest("Postal code is missing!");
            if (!Request.Form.TryGetValue("StreetName", out var streetName))
                return BadRequest("Street name is missing!");
            if (!Request.Form.TryGetValue("HouseNumber", out var houseNumber))
                return BadRequest("House number is missing!");
            if (!Request.Form.TryGetValue("ApartmentNumber", out var apartmentNumber))
                return BadRequest("Apartment number is missing!");
            if (!Request.Form.TryGetValue("Name", out var name))
                return BadRequest("Name is missing!");
            if (!Request.Form.TryGetValue("Surname", out var surname))
                return BadRequest("Surname is missing!");
            if (!Request.Form.TryGetValue("PhoneNumber", out var phoneNumber))
                return BadRequest("Phone number is missing!");
            if (!Request.Form.TryGetValue("Mail", out var email))
                return BadRequest("E-mail is missing!");

            order.AdressDetails = new AdressDetails(orderId, region!, city!, postalCode!, streetName!, houseNumber!, apartmentNumber!);
            order.CustomerDetails = new CustomerDetails(orderId, name!, surname!, phoneNumber!, email!);

            foreach (var item in basket) item.OrderId = orderId;

            _context.Orders.Add(order);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("/api/basket/item/add")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleType.User))]
        public string AddItem()
        {
            return "add";
        }

        [HttpPost("/api/basket/item/remove")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleType.User))]
        public string RemoveItem()
        {
            return "remove";
        }
    }
}
