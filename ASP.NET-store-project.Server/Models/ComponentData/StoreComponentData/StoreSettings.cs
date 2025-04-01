using ASP.NET_store_project.Server.Data.Enums;

namespace ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData
{
    public class StoreSettings
    {
        public ProductCategory SelectedCategory { get; set; }

        public PageSize SelectedPageSize { get; set; }

        public int NumberOfPages { get; set; }

        public int SelectedPageIndex { get; set; }

        public SortingMethod SelectedSortingMethod { get; set; }

        public SortingOrder SelectedSortingOrder { get; set; }

    }
}
