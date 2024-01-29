namespace ASP.NET_store_project.Server.Data
{
    public class Order
    {
        public int Id { get; set; } // Also a foreign key for CustomerDetails and AdressDetails

        public string CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public CustomerDetails CustomerDetails { get; set; } = null!;

        public AdressDetails AdressDetails { get; set; } = null!;

        public List<SelectedItem> OrderedItems { get; } = [];

        public List<OrderStatus> StatusHistory { get; } = [];

    }
}