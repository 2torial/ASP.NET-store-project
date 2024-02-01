using ASP.NET_store_project.Server.Data;
using System.Linq;

namespace ASP.NET_store_project.Server.Models
{
    public class StoreBundle
    {
        public StoreSettings? Settings { get; set; }

        public StoreFilters? Filters { get; set; }

        public StoreItem[]? Items { get; set; }

        public StoreBundle(IFormCollection formData, AppDbContext context)
        {
            var categories = context.Categories.Select(category => category.Label).ToArray();
            string[] sortingMethods = [
                "Name: Ascending",
                "Name: Descending",
                "Price: Lowest first",
                "Price: Highest first",
            ];
            string[] viewModes = ["Gallery", "List"];
            Settings = new StoreSettings()
            {
                Categories = categories,
                SelectedCategory = formData.ContainsKey("Category")
                    ? formData["Category"]
                    : categories.Length > 0 ? categories[0] : "Any",
                SortingMethods = sortingMethods,
                SelectedSortingMethod = sortingMethods[2],
                Pages = 4,
                SelectedPage = 2,
                ViewModes = viewModes,
                ViewModeIcons = Enumerable.Repeat("https://placehold.co/40x40", viewModes.Length).ToArray(),
                SelectedViewMode = viewModes[0],
            };

            var selectedItems = context.Items
                .Where(item => item.Category.Label == Settings.SelectedCategory);
            int min = selectedItems.Min(item => item.Price), 
                max = selectedItems.Max(item => item.Price);
            int from = formData.ContainsKey("From")
                ? int.Parse(formData["From"])
                : int.Max(0, min);
            int to = formData.ContainsKey("To")
                ? int.Parse(formData["To"])
                : int.Max(0, max);
            Filters = new StoreFilters()
            {
                Range = new Dictionary<string, int>() {
                    { "from", from },
                    { "to", to },
                },
                Specifications = selectedItems
                    .SelectMany(item => item.Configurations)
                    .Distinct()
                    .GroupBy(config => config.Label, config => config.Parameter)
                    .ToDictionary(configs => configs.Key, configs => configs.ToArray()),
            };

            Items = selectedItems.Select(item => new StoreItem
            {
                Name = item.Name,
                Price = item.Price,
                Images = item.Gallery.Select(image => image.Content).ToArray(),
                Configuration = new Dictionary<string, string>(item.Configurations //workaround for ToDictionary() inside query
                    .Select(config => new KeyValuePair<string, string>(config.Label, config.Parameter))),
                PageLink = item.Page,
            }).ToArray();
        }
    }
}
