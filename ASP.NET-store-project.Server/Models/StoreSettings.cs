using ASP.NET_store_project.Server.Data;

namespace ASP.NET_store_project.Server.Models
{
    public class StoreSettings
    {
        public List<Category> Categories { get; set; }

        public Category SelectedCategory { get; set; }

        public int Pages { get; set; }

        public int SelectedPage { get; set; }

        public List<SortingMethod> SortingMethods { get; set; }

        public SortingMethod SelectedSortingMethod { get; set; }

        public List<ViewMode> ViewModes { get; set; }

        public ViewMode SelectedViewMode { get; set; }

        public class ViewMode(string mode, string icon)
        {
            public string Mode { get; set; } = mode;

            public string Icon { get; set; } = icon;
        }

        public abstract class SortingMethod(string label) {
            public string Label { get; set; } = label;
        }

        public class SortingMethod<T>(string label, Func<Item,T> query, bool incrementally = true) : SortingMethod(label)
        {
            public Func<Item,T> Query { get; set; } = query;

            public bool Incrementally { get; set; } = incrementally;
        }
    }
}
