namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class AdresseeDetails(string name, string surname, string phoneNumber, string region, string city, string postalCode, string streetName, string houseNumber, string? apartmentNumber = null)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = name;

        public string Surname { get; set; } = surname;

        public string PhoneNumber { get; set; } = phoneNumber;

        public string Region { get; set; } = region;

        public string City { get; set; } = city;

        public string PostalCode { get; set; } = postalCode;

        public string StreetName { get; set; } = streetName;

        public string HouseNumber { get; set; } = houseNumber;

        public string? ApartmentNumber { get; set; } = apartmentNumber;

    }
}