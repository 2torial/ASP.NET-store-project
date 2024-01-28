using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class AdressDetails
    {
        [Key]
        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;

        public string Region { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Street { get; set; }

    }
}