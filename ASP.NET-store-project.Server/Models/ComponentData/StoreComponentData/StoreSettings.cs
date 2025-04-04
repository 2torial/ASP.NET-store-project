using ASP.NET_store_project.Server.Data.Enums;

namespace ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData
{
    public class StoreSettings
    {
        public ProductCategory Category { get; set; }

        public PageSize PageSize { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public SortingMethod SortingMethod { get; set; }

        public SortingOrder SortingOrder { get; set; }

    }
}
