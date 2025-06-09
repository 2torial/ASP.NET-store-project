namespace ASP.NET_store_project.Server.Models.ComponentData;

using StructuredData;

// Class representing React component
public record OrderListComponentData(IEnumerable<OrderInfo> Orders);
