using ASP.NET_store_project.Server.Controllers.StoreController;
using ASP.NET_store_project.Server.Data;
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
        [HttpPost("/api/supplier/A/filter")]
        public IActionResult FilteredProducts([FromBody] PageReloadData pageData)
        {
            // Category filtering
            var categorizedProducts = context.Items
                .Where(item => item.Category.Type == pageData.Category.GetDisplayName())
                .Where(item => !item.IsDeleted)
                .Where(item => pageData.PriceRange.From <= item.Price && item.Price <= pageData.PriceRange.To)
                .Include(item => item.Configurations)
                .Select(item => new ProductInfo(item.Id, item.Name, item.Price) 
                { 
                    Tags = item.Configurations.Select(config => new ProductTag { Label = config.Label, Parameter = config.Parameter }) 
                })
                .AsEnumerable();

            var searchedProducts = categorizedProducts
                .Where(item => pageData.SearchBar.ToLower().Split(null)
                    .All(word => item.Name.Contains(word, StringComparison.CurrentCultureIgnoreCase)));

            // Specification filtering
            var filteredProducts = categorizedProducts
                .Where(item => pageData.RelatedTags
                    .All(kvp => kvp.Value.Any(tag => item.Tags.Contains(tag))))
                .Select(item => new ProductInfo(item.Id, item.Name, item.Price));

            return Ok(filteredProducts);
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
