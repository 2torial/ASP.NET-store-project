using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData;
using ASP.NET_store_project.Server.Models.StructuredData;
using ASP.NET_store_project.Server.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ASP.NET_store_project.Server.Controllers.StoreController
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController(AppDbContext context, IHttpClientFactory httpClientFactory) : ControllerBase
    {
        [HttpPost("/api/reload")]
        public async Task<IActionResult> Reload([FromForm] PageReloadData pageData)
        {
            Console.WriteLine(pageData.Category);
            var suppliers = context.Suppliers
                .Select(sup => new
                    {
                        sup.Id,
                        Client = httpClientFactory.CreateClient(sup.Name),
                        sup.FilteredProductsRequestAdress,
                        sup.SelectedProductsRequestAdress
                    })
                .AsEnumerable();

            var filterRequestClientsData = suppliers
                .ToDictionary(
                    sup => sup.Id,
                    sup => new ClientData(sup.Client)
                    {
                        Content = new StringContent(JsonSerializer.Serialize(pageData), Encoding.UTF8, "application/json"),
                        RequestAdress = sup.FilteredProductsRequestAdress,
                    });

            var categorizedProductsBatch = await MultipleRequestsAsJsonEndpoint<IEnumerable<ProductInfo>>
                .SendAsync(filterRequestClientsData);
            var categorizedProducts = categorizedProductsBatch
                .SelectMany(
                    kvp => kvp.Value ?? [],
                    (batch, prod) =>  new ProductInfo(prod.Id, prod.Name, prod.Price) { SupplierId = batch.Key, Tags = prod.Tags });

            var viablePriceRange = new PriceRange(
                categorizedProducts.Min(prod => prod.Price),
                categorizedProducts.Max(prod => prod.Price));
            var selectedPriceRange = new PriceRange(
                Math.Max(viablePriceRange.From, pageData.PriceFrom),
                Math.Min(viablePriceRange.To, pageData.PriceTo));

            var filteredProducts = categorizedProducts
                .Where(prod => selectedPriceRange.IsInRange(prod.Price));

            var adjustedPriceRange = new PriceRange(
                filteredProducts.Min(prod => prod.Price),
                filteredProducts.Max(prod => prod.Price));

            var keyWords = pageData.SearchBar?.ToLower().Split(null) ?? [];
            filteredProducts = filteredProducts
                .Where(prod => keyWords.All(word => word.Contains(prod.Name)));

            var viableTags = categorizedProducts
                .SelectMany(prod => prod.Tags ?? [])
                .Distinct(new ProductTagComparer());
            var groupedViableTags = viableTags
                .GroupBy(
                    tag => tag.Label,
                    tag => tag,
                    (label, groupedTags) => new KeyValuePair<string, IEnumerable<ProductTag>>(label, groupedTags.OrderBy(tag => tag.Order)))
                .ToDictionary();

            var selectedTags = viableTags
                .Where(tag =>
                    Request.Form.TryGetValue(tag.Label, out var parameters) &&
                    parameters.Contains(tag.Parameter));
            var groupedSelectedTags = selectedTags
                .GroupBy(
                    tag => tag.Label,
                    tag => tag,
                    (label, groupedTags) => new KeyValuePair<string, IEnumerable<ProductTag>>(label, groupedTags.OrderBy(tag => tag.Order)))
                .ToDictionary();

            filteredProducts = filteredProducts
                .Where(prod => groupedSelectedTags.All(kvp => kvp.Value.Any(tag => prod.Tags?.Contains(tag) ?? false)));

            filteredProducts = pageData.ModifyAwaited(filteredProducts);

            var selectedProductsData = filteredProducts
                .GroupBy(
                    prod => prod.SupplierId,
                    prod => prod.Id,
                    (supId, prods) => new KeyValuePair<Guid, string>(supId, JsonSerializer.Serialize(prods)));

            var selectRequestClientsData = suppliers
                .Join(selectedProductsData,
                    sup => sup.Id,
                    selectedData => selectedData.Key,
                    (sup, selectedData) => new KeyValuePair<Guid, ClientData>(sup.Id, new ClientData(sup.Client)
                    {
                        Content = new StringContent(selectedData.Value, Encoding.UTF8, "application/json"),
                        RequestAdress = sup.SelectedProductsRequestAdress,
                    }))
                .ToDictionary();

            var selectedProductsBatch = await MultipleRequestsAsJsonEndpoint<List<ProductInfo>>
                .SendAsync(selectRequestClientsData);
            var selectedProducts = selectedProductsBatch
                .SelectMany(
                    kvp => kvp.Value ?? [], 
                    (batch, prod) => new ProductInfo(prod.Id, prod.Name, prod.Price)
                    {
                        SupplierId = batch.Key,
                        Tags = prod.Tags,
                        Gallery = prod.Gallery,
                        WebPageLink = prod.WebPageLink
                    });

            var storeComponentData = new StoreComponentData
            {
                Settings = new StoreSettings
                {
                    Category = pageData.Category,
                    PageSize = pageData.PageSize,
                    PageCount = !filteredProducts.Any() ? 0 : (filteredProducts.Count() - 1) % pageData.NumericPageSize() + 1,
                    PageIndex = pageData.PageIndex,
                    SortingMethod = pageData.SortBy,
                    SortingOrder = pageData.OrderBy
                },
                Filters = new StoreFilters
                {
                    ViablePriceRange = viablePriceRange,
                    PriceRange = adjustedPriceRange,
                    GroupedTags = groupedViableTags
                },
                Products = selectedProducts
            };

            return Ok(storeComponentData);
        }
    }
}
