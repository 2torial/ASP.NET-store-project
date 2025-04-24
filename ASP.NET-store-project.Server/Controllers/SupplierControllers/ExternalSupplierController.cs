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
        [HttpGet("/api/supplier/{supplierKey}/filter/{category}")] // supplierKey is not part of an API, it is used solely for this project to differentiate "virtual" suppliers
        public IActionResult CategorizedProducts([FromRoute] ProductCategory category, [FromRoute] string supplierKey)
        {
            // Category filtering
            var categorizedProducts = context.Items
                .Where(item => item.SupplierKey == supplierKey) // Technically it's not a part of this API, that is a trick to keep "external APIs" localy
                .Where(item => item.Category.Type == category.GetDisplayName())
                .Where(item => !item.IsDeleted)
                .Where(item => item.Configurations.Count != 0)
                .Include(item => item.Configurations)
                .AsEnumerable()
                .Select(item => new ProductInfo() 
                {
                    Name = item.Name, 
                    Price = item.Price,
                    Id = item.Id.ToString(),
                    Tags = item.Configurations.Select(config => new ProductTag { Label = config.Label, Parameter = config.Parameter, Order = config.Order })
                });

            return Ok(categorizedProducts);
        }

        [HttpPost("/api/supplier/{supplierKey}/select")]
        public IActionResult SelectedProducts([FromBody] IEnumerable<ProductInfo> selectedProductInfos, [FromRoute] string supplierKey)
        {
            var selectedIds = selectedProductInfos
                .Select(prod => prod.Id);
                
            var selectedProducts = context.Items
                .Where(item => item.SupplierKey == supplierKey) // Technically it's not a part of this API, that is a trick to keep "external APIs" localy
                .Where(item => selectedIds.Contains(item.Id.ToString()))
                .Include(item => item.Configurations)
                .AsEnumerable()
                .Select(item => new ProductInfo()
                {
                    Name = item.Name,
                    Price = item.Price,
                    Gallery = [],
                    Tags = item.Configurations.Select(config => new ProductTag { Label = config.Label, Parameter = config.Parameter, Order = config.Order }),
                    PageContent = item.WebPage,
                });

            return Ok(selectedProducts);
        }
    }
}
