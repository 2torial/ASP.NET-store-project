namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class ContactDetails(string name, string surname, string phoneNumber, string email)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = name;

        public string Surname { get; set; } = surname;

        public string PhoneNumber { get; set; } = phoneNumber;

        public string Email { get; set; } = email;

    }
}