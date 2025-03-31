using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData
{
    public class StoreComponentData
    {
        public StoreSettings Settings { get; init; }

        public StoreFilters Filters { get; init; }

        public IEnumerable<ProductInfo> Products { get; init; }

    }
}
