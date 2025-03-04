using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.DataRevised
{
    public class AdressDetails(Guid customerId, string region, string city, string postalCode, string streetName, string houseNumber, string? apartmentNumber = null)
    {
        [Key]
        public Guid Id {get; set; }

        public Guid CustomerId { get; set; } = customerId;

        public string Region { get; set; } = region;

        public string City { get; set; } = city;

        public string PostalCode { get; set; } = postalCode;

        public string StreetName { get; set; } = streetName;

        public string HouseNumber { get; set; } = houseNumber;

        public string? ApartmentNumber { get; set; } = apartmentNumber;

    }
}