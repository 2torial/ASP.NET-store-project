namespace ASP.NET_store_project.Server.Models
{
    public class StoreItems
    {
        public int NumberOfItems { get; set; }

        public List<Item> DisplayedItems { get; set; }

        public class Item {
            public string Name { get; set; }

            public int Price { get; set; }

            public List<string> Gallery { get; set; } // To be changed to List<Base64>

            public List<Configuration> Specification { get; set; }

            public string? PageLink { get; set; }

            public class Configuration
            {
                public string Label { get; set; }

                public string Parameter { get; set; }
            }
        }
    }
}
