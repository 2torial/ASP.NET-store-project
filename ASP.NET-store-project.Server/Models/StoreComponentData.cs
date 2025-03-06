using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.Enums;

namespace ASP.NET_store_project.Server.Models
{
    public class StoreComponentData
    {
        private static List<SortingMethod> SortingMethods { get; } = Enum.GetValues(typeof(SortingMethod)).Cast<SortingMethod>().ToList();


        public StoreSettings Settings { get; }

        public StoreFilters Filters { get; }

        public StoreItems Items { get; }

        public StoreComponentData(IFormCollection formData, AppDbContext context)
        {
            // Sorting method selection
            SortingMethod sortingMethod = formData.TryGetValue("SortBy", out var sortBy)
                ? SortingMethods.Where(sm => sm == sortBy).SingleOrDefault()
                : SortingMethod.Name;
            bool isAscending = formData.TryGetValue("OrderBy", out var orderBy)
                ? orderBy switch {
                    var _ when orderBy == "Ascending" => true,
                    var _ when orderBy == "Descending" => false,
                    _ => true
                } : true;

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

                SortingMethods = SortingMethods.Select(sm => sm.ToString()).ToList(),
                SelectedSortingMethod = sortingMethod.ToString(),
            };

            // Category filtering
            var selectedItems = Settings.SelectedCategory.Type == "Any"
                ? context.Items
                : context.Items.Where(item => item.Category.Type == Settings.SelectedCategory.Type);

            // Not showing items tagged as deleted
            selectedItems = selectedItems.Where(item => !item.IsDeleted);

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
                foreach (var word in searchbar.ToString().ToLower().Split(' '))
                    selectedItems = selectedItems.Where(item => item.Name.ToLower().Contains(word));

            // Price range filtering
            selectedItems = selectedItems
                .Where(item => Filters.PriceRange.From <= item.Price && item.Price <= Filters.PriceRange.To);

            // Specification filtering
            var selectedConfigs = selectedItems
                .SelectMany(item => item.Configurations)
                .Where(config => formData.Keys.Contains(config.Label));
            foreach (var config in selectedConfigs)
                if (formData.TryGetValue(config.Label, out var parameters))
                    selectedItems = selectedItems
                        .Where(item => item.Configurations
                            .Any(config => parameters.ToString()
                                .IndexOf(config.Parameter) >= 0));

            // Sorting
            selectedItems = sortingMethod switch
            {
                SortingMethod.Name => selectedItems.OrderBy(item => item.Name),
                SortingMethod.Price => selectedItems.OrderBy(item => item.Price),
                _ => selectedItems.OrderBy(item => item.Name)
            };

            if (isAscending)
                selectedItems = selectedItems.Reverse();

            Items = new StoreItems
            {
                NumberOfItems = selectedItems.Count(),
                DisplayedItems = selectedItems
                    .Select(item => new StoreItems.Item
                    {
                        Id = item.Id,
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
