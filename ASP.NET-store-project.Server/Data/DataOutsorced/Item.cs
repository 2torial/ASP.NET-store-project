namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Item(string categoryId, string name, decimal price, int quantity, string pageContent = "", bool isAvaliable = false)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string SupplierKey { get; set; } // Not part of the API, it's essential to create "virtual suppliers" locally

        public string CategoryId { get; set; } = categoryId;

        public string Name { get; set; } = name;

        public decimal Price { get; set; } = price;

        public int Quantity { get; set; } = quantity;

        public string PageContent { get; set; } = pageContent;

        public bool IsAvaliable { get; set; } = isAvaliable;



        public ItemCategory Category { get; set; } = null!;

        public List<Configuration> Configurations { get; } = [];

        public List<Image> Gallery { get; } = [];
    }
}