using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData
{
    public class BasketComponentData
    {
        public required IEnumerable<ProductInfo> Products { get; set; }

    }
}
