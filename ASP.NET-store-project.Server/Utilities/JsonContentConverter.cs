namespace ASP.NET_store_project.Server.Utilities;

using System.Text;
using System.Text.Json;

// Sugar class used to create JSON-type StringContent
public static class JsonContentConverter
{
    public static StringContent Convert<T>(T obj) =>
        new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
}
