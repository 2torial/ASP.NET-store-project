namespace ASP.NET_store_project.Server.Models.StructuredData
{
    public class PriceRange
    {
        private decimal _from, _to;
        public decimal From
        {
            get => _from;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                ArgumentOutOfRangeException.ThrowIfGreaterThan(value, To);
                _from = value;
            }
        }
        public decimal To
        {
            get => _to;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                ArgumentOutOfRangeException.ThrowIfLessThan(value, From);
                _to = value;
            }
        }

        public PriceRange(decimal from = 0, decimal to = decimal.MaxValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(from);

            From = from;
            To = to;
        }
    }
}
