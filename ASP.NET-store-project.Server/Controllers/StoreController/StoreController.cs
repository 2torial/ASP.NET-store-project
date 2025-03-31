using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData;
using ASP.NET_store_project.Server.Models.StructureData;
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
            var clients = context.Suppliers
                .ToDictionary(sup => sup.Id, sup => httpClientFactory.CreateClient(sup.Name));
            var content = Request.ReadFormAsync().Result
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
            var contentEncoded = new FormUrlEncodedContent(content);
            
            var requestAdress = context.Suppliers.Single().FilteredProductsRequestAdress;
            var filteredProductsBatch = await MultipleRequestsAsJsonEndpoint<List<ProductInfo>>
                .SendAsync(clients, requestAdress, contentEncoded);
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
                    (supId, prods) => new KeyValuePair<Guid, string>(supId, JsonSerializer.Serialize(prods)))
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => (clients[kvp.Key], new StringContent(kvp.Value, Encoding.UTF8, "application/json")));

            requestAdress = context.Suppliers.Single().SelectedProductsRequestAdress;
            var selectedProductsBatch = await MultipleRequestsAsJsonEndpoint<List<ProductInfo>>
                .SendAsync(selectedProductsData, requestAdress);
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
                Filters = new StoreFilters(selectedProducts),
                Products = selectedProducts
            };

            return Ok(storeComponentData);
        }
    }
}
