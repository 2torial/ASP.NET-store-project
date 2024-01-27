using ASP.NET_store_project.Server.Data;

namespace ASP.NET_store_project.Server.Data
{
    public class Item
    {
        public int Id { get; set; }

        public string CategoryId { get; set; }

        public Category Type { get; set; } = null!;

        public string Name { get; set; }

        public int Price { get; set; }

        public List<Specification> Specifications { get; } = [];

        public List<Image> Images { get; } = [];

        public string? Page { get; set; }
    }
}