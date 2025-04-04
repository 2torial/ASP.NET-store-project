using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData;
using ASP.NET_store_project.Server.Models.StructuredData;
using ASP.NET_store_project.Server.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

            var filteredProductsBatch = await MultipleRequestsAsJsonEndpoint<List<ProductInfo>>
                .SendAsync(filterRequestClientsData);
            var filteredProducts = filteredProductsBatch
                .SelectMany(
                    kvp => kvp.Value,
                    (batch, prod) =>
                        new ProductInfo(prod.Id, prod.Name, prod.Price) { SupplierId = batch.Key, Tags = prod.Tags });

            filteredProducts = filteredProducts
                .Where(prod => prod.Tags?.All(tag =>
                    !Request.Form.TryGetValue(tag.Label, out var parameters) || // if label is not present, there are no restrictions for the parameter type
                    parameters.Any(param => param == tag.Parameter)) ?? false); // if it is present, product parameters must provide at least one of a kind

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
                .SelectMany(kvp => kvp.Value, (batch, prod) => new ProductInfo(prod.Id, prod.Name, prod.Price)
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
                    PriceRange = new PriceRange(
                        selectedProducts.Min(prod => prod.Price),
                        selectedProducts.Max(prod => prod.Price)),
                    RelatedTags = selectedProducts
                        .SelectMany(prod => prod.Tags ?? [])
                        .Distinct(new ProductTagComparer())
                        .GroupBy(
                            tag => tag.Label,
                            tag => tag,
                            (label, tags) => new KeyValuePair<string, IEnumerable<ProductTag>>(label, tags))
                        .ToDictionary()
                },
                Products = selectedProducts
            };

            return Ok(storeComponentData);
        }
    }
}
