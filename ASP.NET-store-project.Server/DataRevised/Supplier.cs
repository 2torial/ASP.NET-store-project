using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.DataRevised
{
    public class Supplier(string name, Uri uri, decimal profitMultiplier = 1.15m)
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = name;

        public Uri Uri { get; set; } = uri;

        public decimal ProfitMultiplier { get; set; } = profitMultiplier;

    }
}