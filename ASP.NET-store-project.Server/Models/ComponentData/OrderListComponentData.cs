using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData
{
    public class OrderListComponentData
    {
        public required IEnumerable<OrderData> Orders { get; set; }

    }

    public record OrderData(
        Guid SupplierId,
        string SupplierName,
        CustomerInfo CustomerDetails, 
        AdressInfo AdressDetails, 
        IEnumerable<ProductInfo> Products);
}
