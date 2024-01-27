using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class Customer
    {
        [Key]
        public string UserName { get; set; }

        public string PassWord { get; set; }

        public bool IsAdmin { get; set; } = false;

        public List<SelectedItem> Basket { get; } = [];

        public List<Order> Orders { get; } = [];

    }
}