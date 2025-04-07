namespace ASP.NET_store_project.Server.Utilities
{
    public static class ProfitCalculator
    {
        public static decimal Calculate(decimal cost, decimal multiplier)
        {
            var profitBase = cost * multiplier;
            if (profitBase <= 0)
                throw new Exception("No profit exception");
            if (profitBase <= 5)
                profitBase = 5;

            return profitBase + AmountUntilGoodLookingReminder(cost + profitBase);
        }

        private static decimal AmountUntilGoodLookingReminder(decimal price)
        {
            var reminder = Math.Floor(price) - price;
            return reminder switch
            {
                0 or 0.5m or 0.99m => 0,
                > 0 and < 1 => 0.99m - reminder,
                _ => throw new NotImplementedException("Impossible reminder"),
            };
        }
    }
}
