using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController(AppDbContext context) : ControllerBase
    {
        [HttpPost("/api/supply/modified/{supplierName}")]
        public IActionResult SupplyModified()
        {
            return Ok(new StoreComponentData(Request.Form, context));
        }

        [HttpPost("/api/supply/items/{supplierName}")]
        public IActionResult SupplyItems()
        {
            return Ok(new StoreComponentData(Request.Form, context));
        }
    }
}
