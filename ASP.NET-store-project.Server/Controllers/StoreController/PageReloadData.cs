namespace ASP.NET_store_project.Server.Controllers.StoreController;

using Data.Enums;
using Models.StructuredData;
using System.ComponentModel;

// FromForm request data
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
    // Sorts ProductInfo list-type based on recieved data
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

    // Transforms recieved PageSize into numeric format
    public int NumericPageSize() => PageSize switch
    {
        PageSize.Take20 => 20,
        PageSize.Take50 => 50,
        PageSize.Take100 => 100,
        _ => throw new InvalidEnumArgumentException()
    };

    // Calculates number of product pages based on recieved PageSize
    // There is always at least 1 page
    // If negative occurs, the result is 1
    // Subtraction takes place because it's index-based (otherwise it might cause page miscount)
    public int CountPages(int productCount) =>
        1 + Math.Max(0, (productCount - 1) / NumericPageSize());

}
