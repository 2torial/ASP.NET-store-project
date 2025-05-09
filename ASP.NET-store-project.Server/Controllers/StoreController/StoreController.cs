using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Models.ComponentData;
using ASP.NET_store_project.Server.Models.StructuredData;
using ASP.NET_store_project.Server.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_store_project.Server.Controllers.StoreController
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController(AppDbContext context, IHttpClientFactory httpClientFactory) : ControllerBase
    {
        [HttpGet("/api/store/product/{supplierId}/{productId}")]
        public async Task<IActionResult> ShowProduct([FromRoute] string supplierId, [FromRoute] string productId)
        {
            var supplier = context.Suppliers.SingleOrDefault(sup => supplierId == sup.Id.ToString());
            if (supplier == null)
                return BadRequest("Product not found");

            var message = await httpClientFactory
                .CreateClient(supplier.Name)
                .GetAsync($"{supplier.DisplayedProductRequestAdress}/{productId}");

            if (!message.IsSuccessStatusCode)
                return BadRequest("Product not found");

            var content = await message.Content.ReadAsStringAsync();

            return Ok(content);
        }

        [HttpPost("/api/store/reload")]
        public async Task<IActionResult> Reload([FromForm] PageReloadData pageData)
        {
            var suppliers = context.Suppliers.AsEnumerable();

            var categorizedProducts = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .GetAsync(suppliers, 
                    sup => new(
                        httpClientFactory.CreateClient(sup.Name),
                        $"{sup.FilteredProductsRequestAdress}/{pageData.Category}"),
                    (sup, prods) => prods?.Select(prod => prod.NewModified(sup)))
                .ContinueWith(group => group.Result
                    .SelectMany(prods => prods ?? []));

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

            var pageCount = pageData.CountPages(filteredProducts.Count());
            var correctedIndex = Math.Max(1, Math.Min(pageData.PageIndex, pageCount));  

            var selectedProducts = filteredProducts
                .Skip(pageData.NumericPageSize() * (correctedIndex - 1))
                .Take(pageData.NumericPageSize());

            var groupedSelectedProducts = selectedProducts
                .GroupBy(
                    prod => prod.SupplierId,
                    prod => prod,
                    (supId, prods) => new KeyValuePair<Supplier, IEnumerable<ProductInfo>>(suppliers.First(sup => sup.Id == supId), prods));

            selectedProducts = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .PostAsync(groupedSelectedProducts,
                    kvp => new(
                        httpClientFactory.CreateClient(kvp.Key.Name),
                        kvp.Key.SelectedProductsRequestAdress,
                        JsonContentConverter.Convert(kvp.Value)),
                    (kvp, prods) => prods?.Select(prod => prod.NewModified(kvp.Key)))
                .ContinueWith(group => group.Result
                    .SelectMany(prods => prods ?? []));

            return Ok(new StoreComponentData(
                new StoreSettings(
                    pageData.Category, 
                    pageData.PageSize,
                    pageCount,
                    correctedIndex, 
                    pageData.SortBy, 
                    pageData.OrderBy), 
                new StoreFilters(
                    viablePriceRange, 
                    selectedPriceRange, 
                    labeledViableTags), 
                selectedProducts));
        }
    }
}
