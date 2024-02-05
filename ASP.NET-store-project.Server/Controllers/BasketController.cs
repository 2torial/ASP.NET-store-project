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
