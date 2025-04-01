using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models.StructuredData;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers.SupplierControllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierAController(AppDbContext context) : ControllerBase
    {
        [HttpPost("/api/supplier/A/filter")]
        public IActionResult FilteredProducts()
        {
            var category = (Request.Form.TryGetValue("Category", out var type)
                ? context.Categories.Where(category => category.Type == type.ToString()).FirstOrDefault()
                : null) ?? context.Categories
                    .Where(category => category.Type == "Laptops")
                    .Single();

            // Category filtering
            var categorizedProducts = category.Type == "Any"
                ? context.Items
                : context.Items.Where(item => item.Category.Type == category.Type);

            // Not showing items tagged as deleted
            categorizedProducts = categorizedProducts.Where(item => !item.IsDeleted);

            //// Price range min and max values evaluation
            decimal from = 0, to = decimal.MaxValue;
            if (Request.Form.TryGetValue("PriceFrom", out var priceFrom))
                _ = decimal.TryParse(priceFrom.ToString(), out from);
            if (Request.Form.TryGetValue("PriceTo", out var priceTo))
                _ = decimal.TryParse(priceTo.ToString(), out to);
            var priceRange = new PriceRange(from, to);

            // Searchbar filtering
            if (Request.Form.TryGetValue("SearchBar", out var searchbar))
                foreach (var word in searchbar.ToString().ToLower().Split(' '))
                    categorizedProducts = categorizedProducts.Where(item => item.Name.Contains(word, StringComparison.CurrentCultureIgnoreCase));

            // Price range filtering
            categorizedProducts = categorizedProducts
                .Where(item => priceRange.From <= item.Price && item.Price <= priceRange.To);

            // Specification filtering
            var selectedConfigs = categorizedProducts
                .SelectMany(item => item.Configurations)
                .Where(config => Request.Form.Keys.Contains(config.Label));
            foreach (var config in selectedConfigs)
                if (Request.Form.TryGetValue(config.Label, out var parameters))
                    categorizedProducts = categorizedProducts
                        .Where(item => item.Configurations
                            .Any(config => parameters.ToString()
                                .Contains(config.Parameter, StringComparison.CurrentCulture)));

            var filteredProducts = categorizedProducts
                .Select(item => new ProductInfo(item.Id, item.Name, item.Price))
                .AsEnumerable();

            return Ok(filteredProducts);
        }

        [HttpPost("/api/supplier/A/select")]
        public IActionResult SelectedProducts([FromBody] List<Guid> selectedProductIds)
        {
            var selectedProducts = context.Items
                .Where(item => selectedProductIds.Contains(item.Id))
                .AsEnumerable()
                .Select(item => new ProductInfo(item.Id, item.Name, item.Price)
                {
                    Gallery = [],
                    Tags = item.Configurations.Select(config => new ProductTag { Label = config.Label, Parameter = config.Parameter }).ToList(),
                    WebPageLink = item.WebPage,
                });
            return Ok(selectedProducts);
        }
    }
}
