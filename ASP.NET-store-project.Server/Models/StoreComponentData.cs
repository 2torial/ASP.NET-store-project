using ASP.NET_store_project.Server.Data;

namespace ASP.NET_store_project.Server.Models
{
    public class StoreComponentData
    {
        public StoreSettings Settings { get; }

        public StoreFilters Filters { get; }

        public StoreItems Items { get; }

        public StoreComponentData(IFormCollection formData, AppDbContext context)
        {
            var sortingMethod = (formData.TryGetValue("SortBy", out var sortBy)
                ? context.SortingMethods.Where(method => method.Label == sortBy).FirstOrDefault()
                : null) ?? context.SortingMethods
                    .Where(method => method.Label == "Price: Lowest to Highest")
                    .Single();
            Settings = new StoreSettings
            {
                Categories = context.Categories.ToList(),
                SelectedCategory = (formData.TryGetValue("Category", out var type)
                    ? context.Categories.Where(category => category.Type == type).FirstOrDefault()
                    : null) ?? context.Categories
                        .Where(category => category.Type == "Laptops")
                        .Single(), // Default category set to Laptops for presentation purposes

                Pages = 1,
                SelectedPage = 1,

                SortingMethods = context.SortingMethods
                    .Select(method => method.Label)
                    .ToList(),
                SelectedSortingMethod = sortingMethod.Label,
            };

            var selectedItems = Settings.SelectedCategory.Type == "Any"
                ? context.Items
                : context.Items.Where(item => item.Category.Type == Settings.SelectedCategory.Type);
            if (sortingMethod.IsAscending) selectedItems
                .OrderBy(item => typeof(Item)
                    .GetProperty(sortingMethod.SortBy)!
                    .GetValue(item));
            else selectedItems
                .OrderByDescending(item => typeof(Item)
                    .GetProperty(sortingMethod.SortBy)!
                    .GetValue(item));

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
