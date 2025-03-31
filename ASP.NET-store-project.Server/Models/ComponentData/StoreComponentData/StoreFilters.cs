using ASP.NET_store_project.Server.Models.StructuredData;
using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData
{
    public class StoreFilters(IEnumerable<ProductInfo> products)
    {
        public PriceRange PriceRange { get; set; } = 
            new PriceRange(
                products.Min(prod => prod.Price), 
                products.Max(prod => prod.Price));

        public Dictionary<string, IEnumerable<string>> Configurations { get; set; } = products
            .SelectMany(prod => prod.Tags)
            .Distinct(EqualityComparer<ProductTag>.Create(
                (tag1, tag2) => tag1.Label.Equals(tag2.Label) && tag1.Parameter.Equals(tag2.Parameter)))
            .GroupBy(
                tag => tag.Label,
                tag => tag.Parameter,
                (label, parameters) => new KeyValuePair<string, IEnumerable<string>>(label, parameters))
            .ToDictionary();

    }
}
