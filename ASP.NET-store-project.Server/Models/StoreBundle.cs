namespace ASP.NET_store_project.Server.Models
{
    public class StoreBundle
    {
        public StoreSettings? Settings { get; set; }

        public StoreFilters? Filters { get; set; }

        public StoreItem[]? Items { get; set; }
    }
}
