using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData;
using ASP.NET_store_project.Server.Models.StructuredData;
using ASP.NET_store_project.Server.Utilities;
using ASP.NET_store_project.Server.Utilities.MultipleRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_store_project.Server.Controllers.StoreController
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController(AppDbContext context, IHttpClientFactory httpClientFactory) : ControllerBase
    {
        [HttpPost("/api/reload")]
        public async Task<IActionResult> Reload([FromForm] PageReloadData pageData)
        {
            var suppliers = context.Suppliers.AsEnumerable();

            var categorizedProductsBatch = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .GetAsync(
                    suppliers, 
                    sup => new(
                        httpClientFactory.CreateClient(sup.Name),
                        $"{sup.FilteredProductsRequestAdress}/{pageData.Category}"),
                    (sup, prods) => prods?.Select(prod => new ProductInfo(prod).Modify(sup)));

            var categorizedProducts = categorizedProductsBatch
                .SelectMany(prod => prod ?? []);

            var viablePriceRange = !categorizedProducts.Any()
               ? new PriceRange(0, decimal.MaxValue)
               : new PriceRange(
                   categorizedProducts.Min(prod => prod.Price),
                   categorizedProducts.Max(prod => prod.Price));
            var selectedPriceRange = new PriceRange(
                Math.Max(viablePriceRange.From, pageData.PriceFrom),
                Math.Min(viablePriceRange.To, pageData.PriceTo));

            var viableTags = categorizedProducts
                .SelectMany(prod => prod.Tags ?? [])
                .Distinct(new ProductTagComparer());
            var labeledViableTags = viableTags
                .GroupBy(
                    tag => tag.Label,
                    tag => tag,
                    (label, groupedTags) => new KeyValuePair<string, IEnumerable<ProductTag>>(label, groupedTags))
                .ToDictionary();

            var selectedTags = viableTags
                .Where(tag =>
                    Request.Form.TryGetValue(tag.Label, out var parameters) &&
                    parameters.Contains(tag.Parameter));
            var groupedSelectedTags = selectedTags
                .GroupBy(
                    tag => tag.Label,
                    tag => tag,
                    (label, groupedTags) => groupedTags)
                .AsEnumerable();

            var keyWords = pageData.SearchBar ?? [];
            var filteredProducts = categorizedProducts
                .Where(prod => keyWords
                    .All(keyWord => prod.Name?.Contains(keyWord, StringComparison.CurrentCultureIgnoreCase) ?? false));

            filteredProducts = filteredProducts
                .Where(prod => selectedPriceRange.IsInRange(prod.Price));

            filteredProducts = filteredProducts
                .Where(prod => groupedSelectedTags
                    .All(groupedTags => groupedTags
                        .Any(tag => prod.Tags!.Contains(tag, new ProductTagComparer()))));

            filteredProducts = pageData.Sort(filteredProducts);

            var selectedProducts = pageData.Slice(filteredProducts);

            var groupedSelectedProducts = selectedProducts
                .GroupBy(
                    prod => prod.SupplierId,
                    prod => prod,
                    (supId, prods) => new KeyValuePair<Supplier, IEnumerable<ProductInfo>>(suppliers.First(sup => sup.Id == supId), prods));

            var selectedProductsBatch = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .PostAsync(groupedSelectedProducts,
                    kvp => new(
                        httpClientFactory.CreateClient(kvp.Key.Name),
                        kvp.Key.SelectedProductsRequestAdress,
                        JsonContentConverter.Convert(kvp.Value)),
                    (kvp, prods) => prods?.Select(prod => new ProductInfo(prod).Modify(kvp.Key)));

            selectedProducts = selectedProductsBatch
                .SelectMany(prods => prods ?? []);

            var storeComponentData = new StoreComponentData
            {
                Settings = new StoreSettings
                {
                    Category = pageData.Category,
                    PageSize = pageData.PageSize,
                    PageCount = pageData.CountPages(filteredProducts.Count()),
                    PageIndex = pageData.PageIndex,
                    SortingMethod = pageData.SortBy,
                    SortingOrder = pageData.OrderBy
                },
                Filters = new StoreFilters
                {
                    ViablePriceRange = viablePriceRange,
                    PriceRange = selectedPriceRange,
                    GroupedTags = labeledViableTags
                },
                Products = selectedProducts
            };

            return Ok(storeComponentData);
        }
    }
}
