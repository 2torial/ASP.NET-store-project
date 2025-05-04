using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData
{
    public class StoreComponentData
    {
        public required StoreSettings Settings { get; init; }

        public required StoreFilters Filters { get; init; }

        public required IEnumerable<ProductInfo> Products { get; init; }

    }

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
