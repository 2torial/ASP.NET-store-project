namespace ASP.NET_store_project.Server.Data
{
    public class Configuration
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public string Parameter { get; set; }

        public List<Item> Items { get; } = [];

    }

    public class ItemConfiguration
    {
        public int ItemId { get; set; }

        public int ConfigurationId { get; set; }

    }
}
