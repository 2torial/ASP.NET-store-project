using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController(AppDbContext context) : ControllerBase
    {
        [HttpPost("/api/reload")]
        public IActionResult Reload()
        {
            return Ok(new StoreComponentData(Request.Form, context));
        }
    }
}
