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
    public class ExternalSupplierController(AppDbContext context) : ControllerBase
    {
        [HttpGet("/api/supplier/{supplierKey}/filter/{category}")] // supplierKey is not part of an API it is used solely for this project to differentiate "virtual" suppliers
        public IActionResult CategorizedProducts([FromRoute] ProductCategory category, [FromRoute] string supplierKey)
        {
            Console.WriteLine(supplierKey);
            // Category filtering
            var categorizedProducts = context.Items
                .Where(item => item.SupplierKey == supplierKey) // Technically its prone to SQL Injection but it should not be consider a part of thi API, it's a trick to make calls to "external APIs" kept locally
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

        [HttpPost("/api/supplier/{supplierKey}/select")]
        public IActionResult SelectedProducts([FromBody] List<Guid> selectedProductIds, [FromRoute] string supplierKey)
        {
            var selectedProducts = context.Items
                .Where(item => item.SupplierKey == supplierKey) // Technically its prone to SQL Injection but it should not be consider a part of thi API, it's a trick to make calls to "external APIs" kept locally
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
