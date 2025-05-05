using System.Text.Json.Serialization;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
    [method: JsonConstructor]
    public record OrderInfo(
        string? Id, 
        string? SupplierId,
        string? SupplierName,
        IEnumerable<ProductInfo> Products, 
        CustomerInfo CustomerDetails, 
        AdressInfo AdressDetails, 
        string? Stage)
    {
        public OrderInfo(IEnumerable<ProductInfo> Products, CustomerInfo CustomerDetails, AdressInfo AdressDetails)
            : this(null, null, null, Products, CustomerDetails, AdressDetails, null) { }

    }
}
