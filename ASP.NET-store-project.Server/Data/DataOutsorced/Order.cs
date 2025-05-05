namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Order(Guid adresseeDetailsId, string storeId, string customerId)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string SupplierKey { get; set; } // Not part of the API, it's essential to create "virtual suppliers" localy

        public Guid AdresseeDetailsId { get; set; } = adresseeDetailsId;

        public string StoreId { get; set; } = storeId;

        public string CustomerId { get; set; } = customerId;



        public AdresseeDetails AdresseeDetails { get; set; } = null!;

        public List<Item> Items { get; } = [];

        public List<ItemOrder> ItemOrders { get; } = [];

        public List<Stage> Stages { get; } = [];

    }

    public class ItemOrder(Guid itemId, Guid orderId, decimal storePrice, int quantity)
    {
        public Guid ItemId { get; set; } = itemId;

        public Guid OrderId { get; set; } = orderId;

        public decimal StorePrice { get; set; } = storePrice;

        public int Quantity { get; set; } = quantity;


        public Item Item { get; set; } = null!;

    }

}