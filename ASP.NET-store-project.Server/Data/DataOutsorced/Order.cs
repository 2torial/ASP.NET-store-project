namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Order(Guid adresseeDetailsId, string issuerId)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string SupplierKey { get; set; } // Not part of the API, it's essential to create "virtual suppliers" locally

        public Guid AdresseeDetailsId { get; set; } = adresseeDetailsId;

        public string IssuerId { get; set; } = issuerId;


        public List<Item> Items { get; set; } = [];

        public List<OrderStage> OrderStages { get; } = [];

    }

    public class ItemOrder(Guid itemId, Guid orderId, int quantity)
    {
        public Guid ItemId { get; set; } = itemId;

        public Guid OrderId { get; set; } = orderId;

        public int Quantity { get; set; } = quantity;

    }

}