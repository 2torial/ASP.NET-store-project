using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData.StoreComponentData;
using ASP.NET_store_project.Server.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers.SupplierControllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierBController(AppDbContext context) : ControllerBase
    {
        [HttpPost("/api/supplier/B/filter")]
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
            var priceRange = new PriceRange();
            if (Request.Form.TryGetValue("PriceFrom", out var from))
                if (int.TryParse(from.ToString(), out var priceFrom))
                    priceRange.From = priceFrom;
            if (Request.Form.TryGetValue("PriceTo", out var to))
                if (int.TryParse(to.ToString(), out var priceTo))
                    priceRange.To = priceTo;

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
                .Select(item => new ProductData(item.Id, item.Name, item.Price))
                /*.ToList()*/;

            return Ok(filteredProducts);
        }

        [HttpPost("/api/supplier/B/select")]
        public IActionResult SelectedProducts([FromBody] List<Guid> selectedProductIds)
        {
            var selectedProducts = context.Items
                .Where(item => selectedProductIds.Contains(item.Id))
                .Select(item => new ProductData(item.Id, item.Name, item.Price)
                {
                    Gallery = new List<string>(),
                    Tags = item.Configurations.Select(config => new KeyValuePair<string, string>(config.Label, config.Parameter)).ToList(),
                    WebPageLink = item.WebPage,
                });
            return Ok(selectedProducts);
        }
    }
}
