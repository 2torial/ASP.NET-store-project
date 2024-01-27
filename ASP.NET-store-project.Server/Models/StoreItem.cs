namespace ASP.NET_store_project.Server.Models
{
    public class StoreItem
    {
        public string? Name { get; set; }

        public int Price { get; set; }

        public string[]? Images { get; set; }

        public Dictionary<string, string>? Configuration { get; set; }

        public string? PageLink { get; set; }
    }
}
