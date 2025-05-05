using ASP.NET_store_project.Server.Data.DataRevised;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
    public record ProductInfo(
        string? Id,
        string? Name,
        decimal Price,
        int Quantity,
        Guid? SupplierId,
        IEnumerable<string>? Gallery,
        IEnumerable<ProductTag>? Tags,
        string? PageContent)
    {

        public ProductInfo() : this(null, null, 0, 0, null, null, null, null) { }

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
