using ASP.NET_store_project.Server.Data.DataRevised;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
    public record ProductInfo(
        string? Id,
        string? Name,
        decimal Price,
        int Quantity,
        Guid? SupplierId,
        string? Thumbnail,
        IEnumerable<ProductTag>? Tags,
        string? PageContent)
    {
        public ProductInfo() : this(null, null, 0, 0, null, null, null, null) { }

        public ProductInfo NewModified(Supplier supplier) => 
            new(Id, Name, supplier.CalculateStorePrice(Price), Quantity, supplier.Id, Thumbnail, Tags, PageContent);
    }
}
