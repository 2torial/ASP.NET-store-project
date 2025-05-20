namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Order(Guid adresseeDetailsId, decimal deliveryCost, int deliveryMethod, string storeId, string customerId)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string SupplierKey { get; set; } // Not part of the API, it's essential to create "virtual suppliers" localy

        public Guid AdresseeDetailsId { get; set; } = adresseeDetailsId;

        public decimal DeliveryCost { get; set; } = deliveryCost;

        public int DeliveryMethod { get; set; } = deliveryMethod;

        public string StoreId { get; set; } = storeId;

        public string CustomerId { get; set; } = customerId;



        public AdresseeDetails AdresseeDetails { get; set; } = null!;

        public List<ItemOrder> ItemOrders { get; } = [];

        public List<OrderStage> OrderStages { get; } = [];

    }

    public class ItemOrder(Guid itemId, Guid orderId, decimal storePrice, int quantity, string thumbnailLink)
    {
        public Guid ItemId { get; set; } = itemId;

        public Guid OrderId { get; set; } = orderId;

        public decimal StorePrice { get; set; } = storePrice;

        public int Quantity { get; set; } = quantity;

        public string ThumbnailLink { get; set; } = thumbnailLink;


        public Item Item { get; set; } = null!;

    }

}