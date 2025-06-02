namespace ASP.NET_store_project.Server.Utilities
{
    // Class meant for calculating a profit based on given product cost and some multiplier
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

            return cost + profitBase + AmountUntilGoodLookingReminder(cost + profitBase);
        }

        // Calculates remaining value to achieve .00 or .50 or .99 reminder in cost
        private static decimal AmountUntilGoodLookingReminder(decimal price)
        {
            var reminder = price - Math.Floor(price);
            return reminder switch
            {
                0 or 0.5m or 0.99m => 0, // reminder is already fine
                > 0 and < 1 => 0.99m - reminder, // returns amount to be added
                _ => throw new NotImplementedException("Impossible reminder"),
            };
        }
    }
}
