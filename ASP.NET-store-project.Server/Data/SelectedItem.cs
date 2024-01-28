namespace ASP.NET_store_project.Server.Data
{
    public class SelectedItem
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public Item Item { get; set; } = null!;

        public string CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int Quantity { get; set; }

        public int? OrderId { get; set; }

        public Order? Order { get; set; }

    }
}