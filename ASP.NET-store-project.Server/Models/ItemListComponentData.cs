namespace ASP.NET_store_project.Server.Models
{
    public class ItemListComponentData
    {
        public List<ItemData> Items { get; set; }

        public class ItemData
        {
            public StoreItems.Item Item { get; set; }

            public bool IsDeleted { get; set; }
        }
    }
}
