namespace ASP.NET_store_project.Server.Models.ComponentData;

using StructuredData;

// Class representing React component
public record BasketComponentData(IEnumerable<ProductInfo> Products);
