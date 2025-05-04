using System.Text.Json.Serialization;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
    [method: JsonConstructor]
    public record OrderInfo(
        string? Id, 
        IEnumerable<ProductInfo> Products, 
        CustomerInfo CustomerDetails, 
        AdressInfo AdressDetails, 
        string? OrderStage)
    {
        public OrderInfo(IEnumerable<ProductInfo> Products, CustomerInfo CustomerDetails, AdressInfo AdressDetails)
            : this(null, Products, CustomerDetails, AdressDetails, null) { }

    }
}
