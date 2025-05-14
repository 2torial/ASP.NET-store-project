using System.Text.Json.Serialization;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
    [method: JsonConstructor]
    public record OrderInfo(
        string? Id, 
        string? SupplierId,
        string? SupplierName,
        decimal ProductsCost,
        decimal TransportCost,
        IEnumerable<ProductInfo> Products, 
        CustomerInfo CustomerDetails, 
        AdressInfo AdressDetails, 
        OrderStageInfo[] StageHistory)
    {
        public OrderInfo(IEnumerable<ProductInfo> Products, CustomerInfo CustomerDetails, AdressInfo AdressDetails)
            : this(null, null, null, 0, 0, Products, CustomerDetails, AdressDetails, []) { }

    }

}
