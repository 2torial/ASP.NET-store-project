using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData
{
    // Class representing React component
    public record OrderListComponentData(IEnumerable<OrderInfo> Orders);
}
