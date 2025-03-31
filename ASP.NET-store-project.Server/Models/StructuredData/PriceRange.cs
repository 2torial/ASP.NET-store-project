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
                _from = To < value ? To : value;
            }
        }
        public decimal To
        {
            get => _to;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                _to = value < _from ? From : value;
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
