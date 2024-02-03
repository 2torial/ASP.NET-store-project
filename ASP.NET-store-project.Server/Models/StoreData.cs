using ASP.NET_store_project.Server.Data;

namespace ASP.NET_store_project.Server.Models
{
    public class StoreData
    {
        public StoreSettings Settings { get; }

        public StoreFilters Filters { get; }

        public StoreItems Items { get; }

        public StoreData(IFormCollection formData, AppDbContext context)
        {
            IEnumerable <StoreSettings.ViewMode> viewModes = [
                new("Gallery", "https://placehold.co/20x20"),
                new("List", "https://placehold.co/20x20")
            ];
            IEnumerable<StoreSettings.SortingMethod> sortingMethods = [
                new StoreSettings.SortingMethod<int>("Price: Lowest to Highest", item => item.Price),
                new StoreSettings.SortingMethod<int>("Price: Highest to Lowest", item => item.Price, false),
                new StoreSettings.SortingMethod<string>("Name: Ascending", item => item.Name),
                new StoreSettings.SortingMethod<string>("Name: Descending", item => item.Name, false),
            ];
            Settings = new StoreSettings
            {
                Categories = context.Categories.ToList(),

                SelectedCategory = (formData.TryGetValue("Category", out var type)
                    ? context.Categories.Where(c => c.Type == type).FirstOrDefault()
                    : null) ?? new Category("Any", "Any"),

                Pages = 1,

                SelectedPage = 1,

                SortingMethods = sortingMethods.ToList(),

                SelectedSortingMethod = (formData.TryGetValue("SortBy", out var sort)
                    ? sortingMethods.Where(s => s.Label == sort).FirstOrDefault()
                    : null) ?? sortingMethods.Last(),

                ViewModes = viewModes.ToList(),

                SelectedViewMode = (formData.TryGetValue("ViewMode", out var view)
                    ? viewModes.Where(v => v.Mode == view).FirstOrDefault()
                    : null) ?? viewModes.First(),
            };

            var selectedItems = Settings.SelectedCategory.Type == "Any"
                ? context.Items
                : context.Items.Where(item => item.Category.Type == Settings.SelectedCategory.Type);
            int minPrice = selectedItems.Any()
                ? selectedItems.Min(item => item.Price)
                : 0;
            int maxPrice = selectedItems.Any()
                ? selectedItems.Max(item => item.Price)
                : 0;
            Filters = new StoreFilters
            {
                PriceRange = new StoreFilters.ValueRange
                {
                    From = formData.TryGetValue("From", out var from)
                        ? int.TryParse(from, out var fromValue)
                            ? Math.Max(minPrice, Math.Min(fromValue, maxPrice))
                            : minPrice
                        : minPrice,
                    To = formData.TryGetValue("To", out var to)
                        ? int.TryParse(to, out var toValue)
                            ? Math.Min(Math.Max(minPrice, toValue), maxPrice)
                            : maxPrice
                        : maxPrice,
                },
                Configurations = selectedItems
                    .SelectMany(item => item.Configurations)
                    .Distinct()
                    .OrderBy(config => config.Order)
                    .GroupBy(
                        config => config.Label,
                        config => config.Parameter,
                        (label, parameters) => new StoreFilters.PossibleConfiguration
                        {
                            Label = label,
                            Parameters = parameters.ToList()
                        })
                    .ToList(),
            };

            Items = new StoreItems
            {
                NumberOfItems = selectedItems.Count(),
                DisplayedItems = selectedItems
                    .Select(item => new StoreItems.Item
                    {
                        Name = item.Name,
                        Price = item.Price,
                        Gallery = item.Gallery
                            .Select(image => image.Content)
                            .ToList(),
                        Specification = item.Configurations
                            .Select(config => new StoreItems.Item.Configuration
                            {
                                Label = config.Label,
                                Parameter = config.Parameter,
                            })
                            .ToList(),
                        PageLink = item.Page,
                    }).ToList(),
            };
        }
    }
}
