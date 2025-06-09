namespace ASP.NET_store_project.Server.Models.StructuredData;

using DMethod = Data.Enums.DeliveryMethod; // Name collision within the class
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Extensions;

// Uniform class for server-server and server-client communication
[method: JsonConstructor]
public record OrderInfo(
    string? Id, 
    string? SupplierId,
    string? SupplierName,
    decimal ProductsCost,
    decimal DeliveryCost,
    string DeliveryMethod,
    IEnumerable<ProductInfo> Products, 
    CustomerInfo CustomerDetails, 
    AdressInfo AdressDetails, 
    OrderStageInfo[] StageHistory)
{
    // Minimal contructor required for order summarization
    public OrderInfo(IEnumerable<ProductInfo> products, decimal deliveryCost, CustomerInfo customerDetails, AdressInfo adressDetails)
        : this(null, null, null, 0, deliveryCost, DMethod.Standard.GetDisplayName(), products, customerDetails, adressDetails, []) { }

}
