using ASP.NET_store_project.Server.Data.DataRevised;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
    public class ProductInfo
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Guid? SupplierId { get; set; }

        public IEnumerable<string>? Gallery { get; set; } // To be changed to List<Base64>

        public IEnumerable<ProductTag>? Tags { get; set; }

        public string? PageContent { get; set; }

        public ProductInfo() { }

        public ProductInfo(ProductInfo prev)
        {
            Id = prev.Id;
            Name = prev.Name;
            Price = prev.Price;
            Quantity = prev.Quantity;
            SupplierId = prev.SupplierId;
            Gallery = prev.Gallery;
            Tags = prev.Tags;
            PageContent = prev.PageContent;
        }

        public ProductInfo Modify(Supplier supplier) => new(this)
        {
            Price = supplier.CalculateStorePrice(Price),
            SupplierId = supplier.Id,
        };

    }
}
