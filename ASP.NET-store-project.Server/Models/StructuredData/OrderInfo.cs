using DMethod = ASP.NET_store_project.Server.Data.Enums.DeliveryMethod;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Extensions;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
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
        // Constructor meant for external APIs to fill requested data
        public OrderInfo(IEnumerable<ProductInfo> Products, decimal DeliveryCost, CustomerInfo CustomerDetails, AdressInfo AdressDetails)
            : this(null, null, null, 0, DeliveryCost, DMethod.Standard.GetDisplayName(), Products, CustomerDetails, AdressDetails, []) { }

    }

}
