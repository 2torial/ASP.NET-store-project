using ASP.NET_store_project.Server.Utilities;

namespace Unit_Tests
{
    public class StorePriceCalculatorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        // profit = cost * multiplier
        // if profit > 5: profit = 5
        // storeCost = cost + profit + cleanReminder (.00 or .50 or .99)
        [Test]
        public void TestCalculate()
        {
            // cost or multiplier <= 0 cases (unprofitable)
            int[] trivialParams = [-1, 0, 1];
            var trivialCases = trivialParams
                .SelectMany(_ => trivialParams, (x, y) => (x, y))
                .Where(paramCase => paramCase.x != 1 || paramCase.y != 1);
            foreach (var (cost, multiplier) in trivialCases)
                Assert.Catch<Exception>(() => StorePriceCalculator.Calculate(cost, multiplier));

            decimal[] cleanReminders = [0, 0.5m, 0.99m];

            // baseProfit <= 5 (cases picked to result in clean reminders)
            (decimal, decimal)[] profit5Cases = [(1, 5), (2.5m, 2), (4, 0.25m), (3, 0.01m), (100, 0.04m), (100, 0.05m)];
            foreach (var (cost, multiplier) in profit5Cases)
                Assert.That(StorePriceCalculator.Calculate(cost, multiplier), 
                    Is.EqualTo(cost + 5),
                    $"cost:{cost}|multiplier:{multiplier}");

            // baseProfit <= 5 (cases picked to result in non-clean reminders)
            profit5Cases = [(0.98m, 5), (2.4m, 2), (3.8m, 0.25m), (100.1m, 0.04m), (99.1m, 0.05m)];
            foreach (var (cost, multiplier) in profit5Cases)
                Assert.That(StorePriceCalculator.Calculate(cost, multiplier), 
                    Is.EqualTo(Math.Floor(cost + 5) + 0.99m),
                    $"cost:{cost}|multiplier:{multiplier}");

            // baseProfit >= 5
            var modifiers = Enumerable.Range(1, 99).Select(n => n * 0.01m);
            (decimal, decimal)[] higherProfitCases = [(5, 1), (20, 0.25m), (30, 0.7m), (9999, 0.15m)];

            foreach (var (cost, multiplier) in higherProfitCases)
            {
                foreach (var modifier in modifiers)
                {
                    var modifiedCost = cost + modifier;
                    var profitBase = modifiedCost * multiplier;
                    var result = modifiedCost + profitBase;
                    var reminder = result - Math.Floor(result);
                    if (cleanReminders.Contains(reminder))
                        Assert.That(StorePriceCalculator.Calculate(modifiedCost, multiplier), 
                            Is.EqualTo(result),
                            $"cost:{modifiedCost}|multiplier:{multiplier}");
                    else
                        Assert.That(StorePriceCalculator.Calculate(modifiedCost, multiplier), 
                            Is.EqualTo(Math.Floor(result) + 0.99m),
                            $"cost:{modifiedCost}|multiplier:{multiplier}");
                }
            }
        }
    }
}