using ASP.NET_store_project.Server.Data.Enums;
using ASP.NET_store_project.Server.Models.StructuredData;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace ASP.NET_store_project.Server.Controllers.StoreController
{
    public class PageReloadData
    {
        [FromForm]
        public ProductCategory Category { get; init; }

        [FromForm]
        public decimal PriceFrom { get; init; }
        [FromForm]
        public decimal PriceTo { get; init; }

        [FromForm]
        public string? SearchBar { get; init; }

        [FromForm]
        public SortingMethod SortBy { get; init; }
        [FromForm]
        public SortingOrder OrderBy { get; init; }
        private IEnumerable<ProductInfo> Sort(IEnumerable<ProductInfo> products)
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

        [FromForm]
        public PageSize PageSize { get; init; }
        public int NumericPageSize() => PageSize switch
        {
            PageSize.Take20 => 20,
            PageSize.Take50 => 50,
            PageSize.Take100 => 100,
            _ => throw new InvalidEnumArgumentException()
        };

        [FromForm]
        public int PageIndex { get; init; } = 1;
        private IEnumerable<ProductInfo> Slice(IEnumerable<ProductInfo> products) => products
            .Skip(NumericPageSize() * (PageIndex - 1))
            .Take(NumericPageSize());

        public IEnumerable<ProductInfo> ModifyAwaited(IEnumerable<ProductInfo> products)
        {
            products = Sort(products);
            products = Slice(products);
            return products;
        }
    }
}
