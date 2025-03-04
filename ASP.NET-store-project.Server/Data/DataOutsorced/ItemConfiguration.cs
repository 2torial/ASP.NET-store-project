namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Configuration(int id, string label, string parameter, int order = 9999)
    {
        public int Id { get; set; } = id;

        public string Label { get; set; } = label;

        public string Parameter { get; set; } = parameter;

        public int Order { get; set; } = order;

        public List<Item> Items { get; } = [];

    }

    public class ItemConfiguration(int itemId, int configurationId)
    {
        public int ItemId { get; set; } = itemId;

        public int ConfigurationId { get; set; } = configurationId;

    }
}
