using ASP.NET_store_project.Server.Utilities;

namespace Unit_Tests
{
    // baseProfit = cost * multiplier
    // baseProfit = 5 if baseProfit <= 5
    // storeCost = cost + baseProfit + [something to achieve a clean reminder: .00 or .50 or .99]
    public class StorePriceCalculatorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCalculate_InvalidInput()
        {
            int[] trivialParams = [-1, 0, 1];
            var trivialCases = trivialParams
                .SelectMany(_ => trivialParams, (x, y) => (x, y))
                .Where(paramCase => paramCase.x != 1 || paramCase.y != 1);

            foreach (var (cost, multiplier) in trivialCases)
                Assert.Catch<Exception>(() => StorePriceCalculator.Calculate(cost, multiplier));
        }

        [Test]
        public void TestCalculate_BaseProfitLessThan5_CleanReminders() {
            (decimal, decimal)[] testCases = [(1, 5), (2.5m, 2), (4, 0.25m), (3, 0.01m), (100, 0.04m), (100, 0.05m)];

            foreach (var (cost, multiplier) in testCases)
                Assert.That(StorePriceCalculator.Calculate(cost, multiplier),
                    Is.EqualTo(cost + 5),
                    $"cost:{cost}|multiplier:{multiplier}");
        }

        [Test]
        public void TestCalculate_BaseProfitLessThan5_WithoutCleanReminders()
        {
            (decimal, decimal)[] testCases = [(0.98m, 5), (2.4m, 2), (3.8m, 0.25m), (100.1m, 0.04m), (99.1m, 0.05m)];

            foreach (var (cost, multiplier) in testCases)
                Assert.That(StorePriceCalculator.Calculate(cost, multiplier),
                    Is.EqualTo(Math.Floor(cost + 5) + 0.99m),
                    $"cost:{cost}|multiplier:{multiplier}");
        }

        [Test]
        public void TestCalculate_BaseProfitHigherThan5() {
            decimal[] cleanReminders = [0, 0.5m, 0.99m];
            var modifiers = Enumerable.Range(1, 99).Select(n => n * 0.01m);
            (decimal, decimal)[] testCases = [(5, 1), (20, 0.25m), (30, 0.7m), (9999, 0.15m)];

            foreach (var (cost, multiplier) in testCases)
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