namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Configuration(string label, string parameter, int order = 9999)
    {
        public Guid Id { get; set; }

        public string Label { get; set; } = label;

        public string Parameter { get; set; } = parameter;

        public int Order { get; set; } = order;

        public List<Item> Items { get; } = [];

    }

    public class ItemConfiguration(Guid itemId, Guid configurationId)
    {
        public Guid ItemId { get; set; } = itemId;

        public Guid ConfigurationId { get; set; } = configurationId;

    }
}
