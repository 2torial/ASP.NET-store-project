namespace ASP.NET_store_project.Server.Data
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public CustomerDetails? CustomerDetails { get; set; }

        public AdressDetails? AdressDetails { get; set; }

        public List<SelectedItem> OrderedItems { get; } = [];

        public List<OrderStatus> StatusHistory { get; } = []; 

    }
}