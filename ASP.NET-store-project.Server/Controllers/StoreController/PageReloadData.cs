using ASP.NET_store_project.Server.Controllers.IdentityController;
using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Models.StructuredData;
using FluentValidation;
using System.ComponentModel;

namespace ASP.NET_store_project.Server.Controllers.StoreController
{
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

        public int NumericPageSize() => PageSize switch
        {
            PageSize.Take20 => 20,
            PageSize.Take50 => 50,
            PageSize.Take100 => 100,
            _ => throw new InvalidEnumArgumentException()
        };

        public int CountPages(int productCount) => productCount > 0
            ? 1 + (productCount - 1) / NumericPageSize()
            : 1;

    }
}
