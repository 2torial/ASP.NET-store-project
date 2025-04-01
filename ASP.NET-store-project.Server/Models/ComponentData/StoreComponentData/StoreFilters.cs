using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData
{
    public class StoreFilters
    {
        public PriceRange PriceRange { get; set; }

        public Dictionary<string, IEnumerable<string>> RelatedTags { get; set; }

    }
}
