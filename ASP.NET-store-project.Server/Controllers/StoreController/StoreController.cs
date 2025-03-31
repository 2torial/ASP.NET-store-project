using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.Enums;
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
            var suppliers = context.Suppliers
                .Select(sup => new
                    {
                        sup.Id,
                        Client = httpClientFactory.CreateClient(sup.Name),
                        sup.FilteredProductsRequestAdress,
                        sup.SelectedProductsRequestAdress
                    })
                .AsEnumerable();

            var content = Request.ReadFormAsync().Result
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
            var contentEncoded = new FormUrlEncodedContent(content);

            var filterRequestClientsData = suppliers
                .ToDictionary(
                    sup => sup.Id,
                    sup => new ClientData(sup.Client)
                    {
                        Content = contentEncoded,
                        RequestAdress = sup.FilteredProductsRequestAdress,
                    });

            var filteredProductsBatch = await MultipleRequestsAsJsonEndpoint<List<ProductInfo>>
                .SendAsync(filterRequestClientsData);
            var filteredProducts = filteredProductsBatch
                .SelectMany(
                    kvp => kvp.Value,
                    (batch, prod) =>
                        new ProductInfo(prod.Id, prod.Name, prod.Price) { SupplierId = batch.Key });

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
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value);

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
                    Categories = Enum.GetValues<ProductCategory>(),
                    SelectedCategory = pageData.Category,
                    PageSizes = Enum.GetValues<PageSize>(),
                    SelectedPageSize = pageData.PageSize,
                    NumberOfPages = filteredProducts.Count(),
                    SelectedPage = pageData.PageNumber,
                    SortingMethods = Enum.GetValues<SortingMethod>(),
                    SelectedSortingMethod = pageData.SortBy,
                },
                Filters = new StoreFilters
                {
                    PriceRange = new PriceRange(
                        selectedProducts.Min(prod => prod.Price), 
                        selectedProducts.Max(prod => prod.Price)),
                    Tags = selectedProducts
                        .SelectMany(prod => prod.Tags)
                        .Distinct(EqualityComparer<ProductTag>.Create(
                            (tag1, tag2) => tag1.Label.Equals(tag2.Label) && tag1.Parameter.Equals(tag2.Parameter)))
                        .GroupBy(
                            tag => tag.Label,
                            tag => tag.Parameter,
                            (label, parameters) => new KeyValuePair<string, IEnumerable<string>>(label, parameters))
                        .ToDictionary()
                },
                Products = selectedProducts
            };

            return Ok(storeComponentData);
        }
    }
}
