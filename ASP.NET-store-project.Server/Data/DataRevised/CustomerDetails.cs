using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data.DataRevised
{
    public class CustomerDetails(Guid userId, string name, string surname, string phoneNumber, string email)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; } = userId;

        public string Name { get; set; } = name;

        public string Surname { get; set; } = surname;

        public string PhoneNumber { get; set; } = phoneNumber;

        public string Email { get; set; } = email;

    }
}
