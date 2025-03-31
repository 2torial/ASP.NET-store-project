using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData
{
    public class ItemListComponentData
    {
        public List<ItemData> Items { get; set; }

        public class ItemData
        {
            public ProductInfo Item { get; set; }

            public bool IsDeleted { get; set; }
        }
    }
}
