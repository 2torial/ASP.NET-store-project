namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Item(string categoryId, string name, decimal price, int quantity, string webPage = "", bool isDeleted = false)
    {
        public Guid Id { get; set; }

        public string SupplierKey { get; set; } // Not part of the API, it's essential to create "virtual suppliers" locally

        public string CategoryId { get; set; } = categoryId;

        public string Name { get; set; } = name;

        public decimal Price { get; set; } = price;

        public int Quantity { get; set; } = quantity;

        public string WebPage { get; set; } = webPage;

        public bool IsDeleted { get; set; } = isDeleted;



        public Category Category { get; set; } = null!;

        public List<Configuration> Configurations { get; } = [];

        public List<Image> Gallery { get; } = [];
    }
}