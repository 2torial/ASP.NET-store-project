namespace ASP.NET_store_project.Server.Models.StructuredData
{
    public class PriceRange
    {
        public decimal From { get; private init; }
        public decimal To { get; private init; }

        public PriceRange(decimal from, decimal to)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(from);
            ArgumentOutOfRangeException.ThrowIfNegative(to);
            ArgumentOutOfRangeException.ThrowIfLessThan(to, from);
            From = from;
            To = to;
        }

        public bool IsInRange(decimal value) =>
            From <= value && value <= To;
    }
}
