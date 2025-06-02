using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Models.StructuredData;
using System.ComponentModel;

namespace ASP.NET_store_project.Server.Controllers.StoreController
{
    // FromFrom request data class
    public record PageReloadData(
        ProductCategory Category, 
        decimal PriceFrom, 
        decimal PriceTo, 
        string[]? SearchBar, 
        SortingMethod SortBy, 
        SortingOrder OrderBy, 
        PageSize PageSize,
        int PageIndex)
    {
        // Sorts ProductInfo list-type based on request data
        public IEnumerable<ProductInfo> Sort(IEnumerable<ProductInfo> products)
        {
            products = SortBy switch
            {
                SortingMethod.ByPrice => products.OrderBy(prod => prod.Price),
                SortingMethod.ByName => products.OrderBy(prod => prod.Name),
                _ => throw new InvalidEnumArgumentException()
            };
            products = OrderBy switch
            {
                SortingOrder.Ascending => products,
                SortingOrder.Descending => products.Reverse(),
                _ => throw new InvalidEnumArgumentException()
            };
            return products;
        }

        // Transforms PageSize from request data into numeric format
        public int NumericPageSize() => PageSize switch
        {
            PageSize.Take20 => 20,
            PageSize.Take50 => 50,
            PageSize.Take100 => 100,
            _ => throw new InvalidEnumArgumentException()
        };

        // Calculates number of pages of products based on requested PageSize
        // There is always at least one page
        // If negative occurs, the result is one
        // 1 is subtracted, because index position is calculated
        // Otherwise it might result in first page miscount (example: 1-19 in case of productCount = PageSize = 20)
        public int CountPages(int productCount) =>
            1 + Math.Max(0, (productCount - 1) / NumericPageSize());

    }
}
