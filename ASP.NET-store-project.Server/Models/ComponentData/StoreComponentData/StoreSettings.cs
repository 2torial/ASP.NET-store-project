using ASP.NET_store_project.Server.Data.DataOutsorced;
using ASP.NET_store_project.Server.Data.Enums;

namespace ASP.NET_store_project.Server.Models.ComponentData.StoreComponentData
{
    public class StoreSettings
    {
        public IEnumerable<ProductCategory> Categories { get; set; }

        public ProductCategory SelectedCategory { get; set; }

        public IEnumerable<PageSize> PageSizes { get; set; }

        public PageSize SelectedPageSize { get; set; }

        public int NumberOfPages { get; set; }

        public int SelectedPage { get; set; }

        public IEnumerable<SortingMethod> SortingMethods { get; set; }

        public SortingMethod SelectedSortingMethod { get; set; }
    }
}
