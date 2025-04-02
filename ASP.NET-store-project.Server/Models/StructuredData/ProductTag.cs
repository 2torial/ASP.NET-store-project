using System.Diagnostics.CodeAnalysis;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
    public class ProductTag
    {
        public string Label { get; set; }

        public string Parameter { get; set; }

    }

    public class ProductTagComparer : IEqualityComparer<ProductTag>
    {
        public bool Equals(ProductTag? x, ProductTag? y)
        {
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (x is null || y is null)
                return false;

            //Check whether the products' properties are equal.
            return x.Label == y.Label && x.Parameter == y.Parameter;
        }

        public int GetHashCode([DisallowNull] ProductTag tag)
        {
            //Check whether the object is null
            if (tag is null) return 0;

            //Get hash code for the Name field if it is not null.
            int hashLabel = tag.Label == null ? 0 : tag.Label.GetHashCode();

            //Get hash code for the Code field.
            int hashParameter = tag.Parameter == null ? 0 : tag.Parameter.GetHashCode();

            //Calculate the hash code for the product.
            return hashLabel ^ hashParameter;
        }

    }
}
