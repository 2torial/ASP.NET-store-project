using ASP.NET_store_project.Server.Models.StructuredData;

namespace ASP.NET_store_project.Server.Models.ComponentData
{
    public record UserListComponentData(IEnumerable<UserInfo> Users);
}
