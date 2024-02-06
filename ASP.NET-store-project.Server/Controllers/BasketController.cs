using ASP.NET_store_project.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System.Xml.Linq;

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

            int orderId = _context.Orders.Count() + 1;
            var order = _context.Orders
                .Add(new Order(orderId, customer.Single().UserName));
            foreach (var item in basket)
            {
                item.OrderId = orderId;
            }

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
