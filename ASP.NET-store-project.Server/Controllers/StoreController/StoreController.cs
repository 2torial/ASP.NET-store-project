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
        // Returns selected product's page
        [HttpGet("/api/store/product/{supplierId}/{productId}")]
        public async Task<IActionResult> ShowProduct([FromRoute] string supplierId, [FromRoute] string productId)
        {
            // Identify product's supplier
            var supplier = context.Suppliers.SingleOrDefault(sup => supplierId == sup.Id.ToString());
            if (supplier == null)
                return BadRequest("Product not found");

            // Request product information for display
            var message = await httpClientFactory
                .CreateClient(supplier.Name)
                .GetAsync($"{supplier.DisplayedProductRequestAdress}/{productId}");

            // Abort if product wasn't found
            if (!message.IsSuccessStatusCode)
                return BadRequest("Product not found");

            // Return request result
            var content = await message.Content.ReadAsStringAsync();
            return Ok(content);
        }

        // Reloads main store page containing all of the products that meet selected settings/filters
        [HttpPost("/api/store/reload")]
        public async Task<IActionResult> Reload([FromForm] PageReloadData pageData)
        {
            // Retrieves all of suppliers
            var suppliers = context.Suppliers.AsEnumerable();

            // Request all of the products with given category
            var categorizedProducts = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .GetAsync(suppliers, 
                    sup => new(
                        httpClientFactory.CreateClient(sup.Name),
                        $"{sup.FilteredProductsRequestAdress}/{pageData.Category}"),
                    (sup, prods) => prods?.Select(prod => prod.NewModified(sup)))
                .ContinueWith(group => group.Result
                    .SelectMany(prods => prods ?? []));

            // Determines price range of all those products and adjusts it to fit selected range
            // Used for filtering later in the code
            var viablePriceRange = !categorizedProducts.Any()
               ? new PriceRange(0, decimal.MaxValue)
               : new PriceRange(
                   categorizedProducts.Min(prod => prod.Price),
                   categorizedProducts.Max(prod => prod.Price));
            var selectedPriceRange = new PriceRange(
                Math.Max(viablePriceRange.From, pageData.PriceFrom),
                Math.Min(viablePriceRange.To, pageData.PriceTo));

            // Takes all the tags from those products and removes duplicates
            var viableTags = categorizedProducts
                .SelectMany(prod => prod.Tags ?? [])
                .Distinct(new ProductTagComparer());

            // Groupes all distinct tags by their label (used at the end of the code)
            var labeledViableTags = viableTags
                .GroupBy(
                    tag => tag.Label,
                    tag => tag,
                    (label, groupedTags) => new KeyValuePair<string, IEnumerable<ProductTag>>(label, groupedTags))
                .ToDictionary();

            // Filters user selected tags and groupes them by label
            // Used for filtering later in the code
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

            // Filters products by keywords typed to the searchbar
            var keyWords = pageData.SearchBar ?? [];
            var filteredProducts = categorizedProducts
                .Where(prod => keyWords
                    .All(keyWord => prod.Name?.Contains(keyWord, StringComparison.CurrentCultureIgnoreCase) ?? false));

            // Filters by selected price range
            filteredProducts = filteredProducts
                .Where(prod => selectedPriceRange.IsInRange(prod.Price));

            // Filters by selected tags
            filteredProducts = filteredProducts
                .Where(prod => groupedSelectedTags
                    .All(groupedTags => groupedTags
                        .Any(tag => prod.Tags!.Contains(tag, new ProductTagComparer()))));

            // Sorts using PageReloadData sorting method
            filteredProducts = pageData.Sort(filteredProducts);

            // Counts number of store pages and corrects client's index if out of bound
            var pageCount = pageData.CountPages(filteredProducts.Count());
            var correctedIndex = Math.Max(1, Math.Min(pageData.PageIndex, pageCount));  

            // Selects products for that index
            var selectedProducts = filteredProducts
                .Skip(pageData.NumericPageSize() * (correctedIndex - 1))
                .Take(pageData.NumericPageSize());

            // Groupes selected products by their supplier
            var groupedSelectedProducts = selectedProducts
                .GroupBy(
                    prod => prod.SupplierId,
                    prod => prod,
                    (supId, prods) => new KeyValuePair<Supplier, IEnumerable<ProductInfo>>(suppliers.First(sup => sup.Id == supId), prods));

            // Request missing data from selected products
            selectedProducts = await MultipleRequestsEndpoint<IEnumerable<ProductInfo>>
                .PostAsync(groupedSelectedProducts,
                    kvp => new(
                        httpClientFactory.CreateClient(kvp.Key.Name),
                        kvp.Key.SelectedProductsRequestAdress,
                        JsonContentConverter.Convert(kvp.Value)),
                    (kvp, prods) => prods?.Select(prod => prod.NewModified(kvp.Key)))
                .ContinueWith(group => group.Result
                    .SelectMany(prods => prods ?? []));

            // Returns complete store page data
            return Ok(new StoreComponentData(
                new StoreSettings(      // STORE PAGE SETTINGS
                    pageData.Category,  // Product category
                    pageData.PageSize,  // Number of products per page
                    pageCount,          // Number of pages
                    correctedIndex,     // Page index
                    pageData.SortBy,    // Sorting method
                    pageData.OrderBy),  // Sorting order
                new StoreFilters(       // PRODUCT FILTERS
                    viablePriceRange,   // Viable price range for products of selected category
                    selectedPriceRange, // Selected price range (adjusted to not be out of bounds of viable range)
                    labeledViableTags), // All possible product tags grouped by label
                selectedProducts));     // PRODUCTS THAT FIT THOSE CONDITIONS
        }
    }
}
