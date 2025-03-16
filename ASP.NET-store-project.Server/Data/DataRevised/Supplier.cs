using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data.DataRevised
{
    public class Supplier(string name, Uri uri, decimal profitMultiplier = 0.15m) // 0.10m equals 10% profit
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = name;

        public Uri Uri { get; set; } = uri;

        public decimal ProfitMultiplier { get; set; } = profitMultiplier;

    }
}