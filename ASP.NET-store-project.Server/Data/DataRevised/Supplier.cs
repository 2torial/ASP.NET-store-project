using ASP.NET_store_project.Server.Utilities;

namespace ASP.NET_store_project.Server.Data.DataRevised
{
    public class Supplier(
        string name, 
        string baseAdress, 
        string storeExternalId,
        string filteredProductsRequestAdress, 
        string selectedProductsRequestAdress, 
        string displayedProductRequestAdress,
        string orderListRequestAdress,
        string orderAcceptRequestAdress,
        string orderCancelRequestAdress,
        decimal profitMultiplier = 0.15m) // 0.10m equals 10% profit
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = name;

        public string StoreExternalId { get; set; } = storeExternalId;

        public string BaseAdress { get; set; } = baseAdress;

        public string FilteredProductsRequestAdress { get; set; } = filteredProductsRequestAdress;

        public string SelectedProductsRequestAdress { get; set; } = selectedProductsRequestAdress;

        public string DisplayedProductRequestAdress { get; set; } = displayedProductRequestAdress;

        public string OrderListRequestAdress { get; set; } = orderListRequestAdress;

        public string OrderAcceptRequestAdress { get; set; } = orderAcceptRequestAdress;

        public string OrderCancelRequestAdress { get; set; } = orderCancelRequestAdress;

        public decimal ProfitMultiplier { get; set; } = profitMultiplier;

        public decimal CalculateStorePrice(decimal cost) =>
            StorePriceCalculator.Calculate(cost, ProfitMultiplier);
    }
}