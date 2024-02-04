using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class CustomerDetails(int orderId, string name, string surname, string phoneNumber, string email)
    {
        [Key]
        public int OrderId { get; set; } = orderId;

        public string Name { get; set; } = name;

        public string Surname { get; set; } = surname;

        public string PhoneNumber { get; set; } = phoneNumber;

        public string Email { get; set; } = email;

    }
}
