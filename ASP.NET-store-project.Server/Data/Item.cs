namespace ASP.NET_store_project.Server.Data
{
    public class Item
    {
        public int Id { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public string Name { get; set; }

        public int Price { get; set; }

        public List<Configuration> Configurations { get; } = [];

        public List<Image> Gallery { get; } = [];

        public string? Page { get; set; } // Dummy for item's page content (possible new table)

    }
}