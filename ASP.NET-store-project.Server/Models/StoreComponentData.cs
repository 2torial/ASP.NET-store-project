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
            // Sorting method selection
            var sortingMethod = (formData.TryGetValue("SortBy", out var sortBy)
                ? context.SortingMethods.Where(method => method.Label == sortBy.ToString()).FirstOrDefault()
                : null) ?? context.SortingMethods
                    .Where(method => method.Label == "Price: Lowest to Highest")
                    .Single();
            Settings = new StoreSettings
            {
                Categories = context.Categories.ToList(),
                SelectedCategory = (formData.TryGetValue("Category", out var type)
                    ? context.Categories.Where(category => category.Type == type.ToString()).FirstOrDefault()
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

            // Category filtering
            var selectedItems = Settings.SelectedCategory.Type == "Any"
                ? context.Items
                : context.Items.Where(item => item.Category.Type == Settings.SelectedCategory.Type);

            // Price range min and max values evaluation
            int minPrice = selectedItems.Any()
                ? selectedItems.Min(item => item.Price)
                : 0;
            int maxPrice = selectedItems.Any()
                ? selectedItems.Max(item => item.Price)
                : 0;

            Filters = new StoreFilters
            {
                // Price cannot be set outside of evaluated range
                PriceRange = new StoreFilters.ValueRange
                {
                    From = formData.TryGetValue("PriceFrom", out var from)
                        ? int.TryParse(from.ToString(), out var fromValue)
                            ? Math.Max(minPrice, Math.Min(fromValue, maxPrice))
                            : minPrice
                        : minPrice,
                    To = formData.TryGetValue("PriceTo", out var to)
                        ? int.TryParse(to.ToString(), out var toValue)
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

            // Searchbar filtering
            if (formData.TryGetValue("SearchBar", out var searchbar))
                selectedItems = selectedItems.Where(item => item.Name.Contains(searchbar.ToString()));

            selectedItems = selectedItems
                .Where(item => Filters.PriceRange.From <= item.Price && item.Price <= Filters.PriceRange.To);

            // Sorting
            selectedItems = sortingMethod.SortBy == "Name"
                ? selectedItems.OrderBy(item => item.Name)
                : sortingMethod.SortBy == "Price"
                    ? selectedItems.OrderBy(item => item.Price)
                    : selectedItems;

            if (!sortingMethod.IsAscending)
                selectedItems = selectedItems.Reverse();

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
