namespace ASP.NET_store_project.Server.Models.StructuredData
{
    public class ProductInfo(Guid id, string name, decimal price)
    {
        public Guid Id { get; set; } = id;

        public string Name { get; set; } = name;

        public decimal Price { get; set; } = price;

        public Guid SupplierId { get; set; } = Guid.Empty;

        public string? Thumbnail { get; set; }

        public List<string>? Gallery { get; set; } // To be changed to List<Base64>

        public List<ProductTag>? Tags { get; set; }

        public string? WebPageLink { get; set; }

    }
}
