using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Models.StructuredData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace ASP.NET_store_project.Server.Controllers.SupplierControllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierAController(AppDbContext context) : ControllerBase
    {
        [HttpGet("/api/supplier/A/filter/{category}")]
        public IActionResult CategorizedProducts([FromRoute] ProductCategory category)
        {
            Console.WriteLine("AAA" + category);
            // Category filtering
            var categorizedProducts = context.Items
                .Where(item => item.Category.Type == category.GetDisplayName())
                .Where(item => !item.IsDeleted)
                .Where(item => item.Configurations.Count != 0)
                .Include(item => item.Configurations)
                .AsEnumerable()
                .Select(item => new ProductInfo(item.Id, item.Name, item.Price) 
                { 
                    Tags = item.Configurations.Select(config => new ProductTag { Label = config.Label, Parameter = config.Parameter, Order = config.Order })
                });
            return Ok(categorizedProducts);
        }

        [HttpPost("/api/supplier/A/select")]
        public IActionResult SelectedProducts([FromBody] List<Guid> selectedProductIds)
        {
            var selectedProducts = context.Items
                .Where(item => selectedProductIds.Contains(item.Id))
                .Include(item => item.Configurations)
                .AsEnumerable()
                .Select(item => new ProductInfo(item.Id, item.Name, item.Price)
                {
                    Gallery = [],
                    Tags = item.Configurations.Select(config => new ProductTag { Label = config.Label, Parameter = config.Parameter, Order = config.Order }),
                    WebPageLink = item.WebPage,
                });
            return Ok(selectedProducts);
        }
    }
}
