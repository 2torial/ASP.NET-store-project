using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController(
        AppDbContext context, 
        ILogger<StoreController> logger
    ) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        private readonly ILogger<StoreController> _logger = logger;

        [HttpPost("/api/reload")]
        public StoreComponentData Reload()
        {
            return new StoreComponentData(Request.Form, _context);
        }
    }
}
