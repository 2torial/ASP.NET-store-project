using ASP.NET_store_project.Server.Data;

namespace ASP.NET_store_project.Server.Models
{
    public class BasketComponentData
    {
        public List<BasketedItem> Items { get; set; }

        public class BasketedItem
        {
            public int Id { get; set; }

            public int Quantity { get; set; }

            public string Name { get; set; }

            public int Price { get; set; }

            public List<string> Gallery { get; set; } // To be changed to List<Base64>

            public string? PageLink { get; set; }
        }
    }
}
