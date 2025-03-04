using ASP.NET_store_project.Server.Data.DataRevised;

namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class SelectedItem(int id, int itemId, Guid customerId, int quantity, Guid? orderId = null)
    {
        public int Id { get; set; } = id;

        public int ItemId { get; set; } = itemId;

        public Item Item { get; set; } = null!;

        public Guid CustomerId { get; set; } = customerId;

        public User Customer { get; set; } = null!;

        public int Quantity { get; set; } = quantity;

        public Guid? OrderId { get; set; } = orderId;

        public Order? Order { get; set; }

    }
}