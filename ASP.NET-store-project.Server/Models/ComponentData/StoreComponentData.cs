using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData
{
    public record StoreComponentData(StoreSettings Settings, StoreFilters Filters, IEnumerable<ProductInfo> Products);

    public record StoreFilters(
        PriceRange ViablePriceRange,
        PriceRange PriceRange,
        IDictionary<string, IEnumerable<ProductTag>> GroupedTags);

    public record StoreSettings(
        ProductCategory Category,
        PageSize PageSize,
        int PageCount,
        int PageIndex,
        SortingMethod SortingMethod,
        SortingOrder SortingOrder);
}
