using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController(AppDbContext context) : ControllerBase
    {
        [HttpPost("/api/basket/summary")]
        [Authorize(Policy = IdentityData.RegularUserPolicyName)]
        public IActionResult Summary()
        {
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Where(customer => customer.UserName == jwtToken.Subject);
            if (!customer.Any())
                return BadRequest("This customer does not exist!");

            var basket = customer
                .SelectMany(customer => customer.SelectedItems)
                .Where(item => item.Order == null);

            if (!basket.Any()) return BadRequest("Basket is empty");

            int orderId = context.Orders.Max(order => order.OrderId) + 1;
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

            context.Orders.Add(order);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("/api/basket/add/{itemId}")]
        [Authorize(Policy = IdentityData.RegularUserPolicyName)]
        public IActionResult AddItem([FromRoute] int itemId)
        {
            if (!context.Items.Where(item => item.Id == itemId).Any())
                return BadRequest("This item does not exist!");

            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Where(customer => customer.UserName == jwtToken.Subject);
            if (!customer.Any())
                return BadRequest("This customer does not exist!");

            var selectedItem = customer
                .SelectMany(customer => customer.SelectedItems)
                .Where(item => item.Order == null && item.ItemId == itemId);

            if (selectedItem.Any())
                selectedItem.Single().Quantity += 1;
            else
            {
                int newlySelectedItemId = context.SelectedItems
                    .Max(item => item.Id) + 1;
                context.SelectedItems.Add(
                    new SelectedItem(newlySelectedItemId, itemId, customer.Single().UserName, 1));
            }

            context.SaveChanges();
            return Ok("Added item " + context.Items.Where(item => item.Id == itemId).Single().Name + " (user: " + customer.Single().UserName + ").");
        }

        [HttpGet("/api/basket/remove/{itemId}")]
        [Authorize(Policy = IdentityData.RegularUserPolicyName)]
        public IActionResult RemoveItem([FromRoute] int itemId)
        {
            if (!context.Items.Where(item => item.Id == itemId).Any())
                return BadRequest("This item does not exist!");

            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Where(customer => customer.UserName == jwtToken.Subject);
            if (!customer.Any())
                return BadRequest("This customer does not exist!");

            var selectedItem = customer
                .SelectMany(customer => customer.SelectedItems)
                .Where(item => item.Order == null && item.ItemId == itemId);

            selectedItem.Single().Quantity -= 1;
            if (selectedItem.Single().Quantity == 0)
                context.SelectedItems.Remove(selectedItem.Single());

            context.SaveChanges();
            return Ok("Removed item " + context.Items.Where(item => item.Id == itemId).Single().Name + " (user: " + customer.Single().UserName + ").");
        }

        [HttpGet("/api/basket")]
        [Authorize(Policy = IdentityData.RegularUserPolicyName)]
        public IActionResult Basket()
        {
            var jwtToken = new JwtSecurityToken(Request.Cookies["Token"]);
            var customer = context.Users
                .Where(customer => customer.UserName == jwtToken.Subject);
            if (!customer.Any())
                return BadRequest("This customer does not exist!");

            var basket = customer
                .SelectMany(customer => customer.SelectedItems)
                .Where(item => item.OrderId == null);

            var data = new BasketComponentData
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
            return Ok(data);
        }
    }
}
