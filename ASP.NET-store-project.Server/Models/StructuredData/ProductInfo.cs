using ASP.NET_store_project.Server.Data.DataRevised;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
    public record ProductInfo(
        string? Id,
        string? BasketId,
        string? Name,
        decimal Price,
        int Quantity,
        Guid? SupplierId,
        string? SupplierName,
        string? Thumbnail,
        IEnumerable<ProductTag>? Tags,
        string? PageContent)
    {
        public ProductInfo() : this(null, null, null, 0, 0, null, null, null, null, null) { }

        public ProductInfo NewModified(Supplier supplier, string? basketId = null, bool adjustPrice = true) => 
            new(Id, basketId ?? BasketId, Name, adjustPrice ? supplier.CalculateStorePrice(Price) : Price, Quantity, supplier.Id, supplier.Name, Thumbnail, Tags, PageContent);
    }
}
