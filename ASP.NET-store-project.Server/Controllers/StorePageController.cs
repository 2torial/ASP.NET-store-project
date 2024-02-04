using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorePageController(AppDbContext context, ILogger<StorePageController> logger) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        private readonly ILogger<StorePageController> _logger = logger;

        [HttpPost("/api/reload")]
        public StoreComponentData Reload()
        {
            return new StoreComponentData(Request.Form, _context);
        }
    }
}
