using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.DataRevised
{
    public class CustomerDetails(Guid customerId, string name, string surname, string phoneNumber, string email)
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; } = customerId;

        public string Name { get; set; } = name;

        public string Surname { get; set; } = surname;

        public string PhoneNumber { get; set; } = phoneNumber;

        public string Email { get; set; } = email;

    }
}
