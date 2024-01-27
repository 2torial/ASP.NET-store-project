namespace ASP.NET_store_project.Server.Models
{
    public class StoreFilters
    {
        public Dictionary<string, int>? Range { get; set; }

        public Dictionary<string, string[]>? Specifications { get; set; }
    }
}
