using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData
{
    public class StoreFilters
    {
        public PriceRange PriceRange { get; set; }

        public IEnumerable<RelatedParameters> RelatedTags { get; set; }

    }

    public class RelatedParameters(string label, IEnumerable<string> parameters)
    {
        public string Label { get; set; } = label;

        public IEnumerable<string> Parameters { get; set; } = parameters;

    }
}
