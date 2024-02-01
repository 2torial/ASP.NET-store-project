namespace ASP.NET_store_project.Server.Data
{
    public class SelectedItem(int id, int itemId, string customerId, int quantity, int? orderId = null)
    {
        public int Id { get; set; } = id;

        public int ItemId { get; set; } = itemId;

        public Item Item { get; set; } = null!;

        public string CustomerId { get; set; } = customerId;

        public Customer Customer { get; set; } = null!;

        public int Quantity { get; set; } = quantity;

        public int? OrderId { get; set; } = orderId;

        public Order? Order { get; set; }

    }
}