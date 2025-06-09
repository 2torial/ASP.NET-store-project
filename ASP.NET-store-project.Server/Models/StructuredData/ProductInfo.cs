namespace ASP.NET_store_project.Server.Models.StructuredData;

using Data.DataRevised;

// Uniform class for server-server and server-client communication
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

    // Creates a new instance of the same class with modified values
    // Sets SupplierId and SupplierName according to supplier parameter
    // Resets BasketId if basketId parameter is not null
    // Recalculates Price if adjustPrice is set to true
    public ProductInfo NewModified(Supplier supplier, string? basketId = null, bool adjustPrice = true) => 
        new(Id, basketId ?? BasketId, Name, adjustPrice ? supplier.CalculateStorePrice(Price) : Price, Quantity, supplier.Id, supplier.Name, Thumbnail, Tags, PageContent);
}
