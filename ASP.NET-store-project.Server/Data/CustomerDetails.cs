using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class CustomerDetails
    {
        [Key]
        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

    }
}
