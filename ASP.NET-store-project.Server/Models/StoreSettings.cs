namespace ASP.NET_store_project.Server.Models
{
    public class StoreSettings
    {
        public string[]? Categories { get; set; }

        public string? SelectedCategory { get; set; }

        public string[]? SortingMethods { get; set; }

        public string? SelectedSortingMethod { get; set; }

        public int Pages { get; set; }

        public int SelectedPage { get; set; }

        public string[]? ViewModes { get; set; }

        public string[]? ViewModeIcons { get; set; }

        public string? SelectedViewMode { get; set; }
    }
}
