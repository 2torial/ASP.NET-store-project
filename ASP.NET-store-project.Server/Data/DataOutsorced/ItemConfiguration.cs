namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    // Supplier's database table model
    public class Configuration(string label, string parameter, int order = 9999)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Label { get; set; } = label;

        public string Parameter { get; set; } = parameter;

        public int Order { get; set; } = order;

    }

    // Supplier's database table model
    public class ItemConfiguration(Guid itemId, Guid configurationId)
    {
        public Guid ItemId { get; set; } = itemId;

        public Guid ConfigurationId { get; set; } = configurationId;

    }
}
