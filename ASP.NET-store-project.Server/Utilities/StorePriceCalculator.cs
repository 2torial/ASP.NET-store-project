namespace ASP.NET_store_project.Server.Utilities;

// Used for calculating store prices based on some multiplier
public static class StorePriceCalculator
{
    // Calculates a satisfactory profit and adds it to the cost (resulting amount is going end with .00 or .50 or .99 reminder)
    public static decimal Calculate(decimal cost, decimal multiplier)
    {
        var profitBase = cost * multiplier;
        if (cost <= 0 || multiplier <= 0 || profitBase <= 0)
            throw new Exception("No profit exception");
        if (profitBase <= 5)
            profitBase = 5;

        return PriceWithCleanReminder(cost + profitBase);
    }

    // Returns price with clean reminder (may be slightly higher)
    private static decimal PriceWithCleanReminder(decimal price) => (price - Math.Floor(price)) switch
    {
        0m or 0.5m or 0.99m => price,
        > 0m and < 1m => Math.Floor(price) + 0.99m,
        _ => throw new NotImplementedException("Impossible reminder"),
    };
}
