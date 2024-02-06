using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleType.User))]
        public string Summary()
        {
            return "summary";
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
