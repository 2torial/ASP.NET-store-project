using ASP.NET_store_project.Server.Data;

namespace ASP.NET_store_project.Server.Models
{
    public class StoreSettings
    {
        public List<Category> Categories { get; set; }

        public Category SelectedCategory { get; set; }

        public int Pages { get; set; }

        public int SelectedPage { get; set; }

        public List<string> SortingMethods { get; set; }

        public string SelectedSortingMethod { get; set; }
    }
}
