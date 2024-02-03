using ASP.NET_store_project.Server.Data;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ASP.NET_store_project.Server.Models
{
    public class StoreBundle
    {
        public StoreSettings Settings { get; set; }

        public StoreFilters Filters { get; set; }

        public StoreItem[] Items { get; set; }

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
                    : categories.Length > 0 ? categories[1] : "Any",
                SortingMethods = sortingMethods,
                SelectedSortingMethod = sortingMethods[2],
                Pages = 4,
                SelectedPage = 2,
                ViewModes = viewModes,
                ViewModeIcons = Enumerable.Repeat("https://placehold.co/40x40", viewModes.Length).ToArray(),
                SelectedViewMode = viewModes[0],
            };

            var selectedItems = context.Items
                .Where(item => item.Category.Label == Settings.SelectedCategory)
                .Select(item => new
                {
                    item.Name,
                    item.Price,
                    GalleryArray = item.Gallery.Select(image => image.Content).ToArray(),
                    ConfigurationsList = item.Configurations.Select(config => new
                    {
                        config.Label,
                        config.Parameter
                    }).ToList(),
                    item.Page
                }).ToList();
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
                    .SelectMany(item => item.ConfigurationsList)
                    .Distinct()
                    .GroupBy(config => config.Label, config => config.Parameter)
                    .ToDictionary(configs => configs.Key, configs => configs.ToArray()),
            };

            var Items = selectedItems
                .Select(item => new StoreItem
                {
                    Name = item.Name,
                    Price = item.Price,
                    Images = item.GalleryArray,
                    Configuration = new Dictionary<string, string>(),
                    PageLink = item.Page
                }).ToList();
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Configuration = selectedItems[i].ConfigurationsList
                    .ToDictionary(config => config.Label, config => config.Parameter);
            }

            //selectedItems.Select(item => new StoreItem
            //{
            //    Name = item.Name,
            //    Price = item.Price,
            //    Images = item.GalleryArray,
            //    Configuration = item.ConfigurationsList
            //        .ToDictionary(config => config.Label, config => config.Parameter),
            //    PageLink = item.Page,
            //}).ToArray()
        }
    }
}
