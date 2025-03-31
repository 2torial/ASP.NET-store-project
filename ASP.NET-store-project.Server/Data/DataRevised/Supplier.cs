using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data.DataRevised
{
    public class Supplier(string name, string baseAdress, string filteredProductsRequestAdress, string selectedProductsRequestAdress, decimal profitMultiplier = 0.15m) // 0.10m equals 10% profit
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = name;

        public string BaseAdress { get; set; } = baseAdress;

        public string FilteredProductsRequestAdress { get; set; } = filteredProductsRequestAdress;

        public string SelectedProductsRequestAdress { get; set; } = selectedProductsRequestAdress;

        public decimal ProfitMultiplier { get; set; } = profitMultiplier;

    }
}