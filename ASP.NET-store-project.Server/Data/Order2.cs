using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class Order2(int orderId, string customerId)
    {
        [Key]
        public int OrderId { get; set; } = orderId; // Also a foreign key for CustomerDetails and AdressDetails

        public string CustomerId { get; set; } = customerId;

        public User Customer { get; set; } = null!;

        public CustomerDetails CustomerDetails { get; set; } = null!;

        public AdressDetails AdressDetails { get; set; } = null!;

        public List<SelectedItem> OrderedItems { get; } = [];

        public List<Status> StatusHistory { get; } = [];

        public List<OrderStatus> StatusChangeHistory { get; } = [];

    }
}