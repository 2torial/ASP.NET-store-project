using ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData.StoreComponentData;
using ASP.NET_store_project.Server.Models.StructureData;

namespace ASP.NET_store_project.Server.Models.ComponentData
{
    public class ItemListComponentData
    {
        public List<ItemData> Items { get; set; }

        public class ItemData
        {
            public ProductInfo.Product Item { get; set; }

            public bool IsDeleted { get; set; }
        }
    }
}
