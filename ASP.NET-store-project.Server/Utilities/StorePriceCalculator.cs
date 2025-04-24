namespace ASP.NET_store_project.Server.Utilities
{
    public static class StorePriceCalculator
    {
        public static decimal Calculate(decimal cost, decimal multiplier)
        {
            var profitBase = cost * multiplier;
            if (profitBase <= 0)
                throw new Exception("No profit exception");
            if (profitBase <= 5)
                profitBase = 5;

            var profit = profitBase + AmountUntilGoodLookingReminder(cost + profitBase);
            return cost + profit;
        }

        private static decimal AmountUntilGoodLookingReminder(decimal price)
        {
            var reminder = Math.Ceiling(price) - price;
            return reminder switch
            {
                0 or 0.5m or 0.99m => 0,
                > 0 and < 1 => 0.99m - reminder,
                _ => throw new NotImplementedException("Impossible reminder"),
            };
        }
    }
}
