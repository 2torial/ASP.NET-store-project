namespace ASP.NET_store_project.Server.Data
{
    public class Item(int id, string categoryId, string name, int price, string? page = null)
    {
        public int Id { get; set; } = id;

        public string CategoryId { get; set; } = categoryId;

        public Category Category { get; set; } = null!;

        public string Name { get; set; } = name;

        public int Price { get; set; } = price;

        public List<Configuration> Configurations { get; } = [];

        public List<Image> Gallery { get; } = [];

        public string? Page { get; set; } = page;

    }
}