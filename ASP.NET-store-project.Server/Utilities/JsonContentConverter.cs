using System.Text;
using System.Text.Json;

namespace ASP.NET_store_project.Server.Utilities
{
    public static class JsonContentConverter
    {
        public static StringContent Convert<T>(T obj) =>
            new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }
}
