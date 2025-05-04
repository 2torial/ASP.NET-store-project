using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers.BasketController
{
    public class OrderSummaryData
    {
        [FromForm]
        public required string Name { get; set; }

        [FromForm]
        public required string Surname { get; set; }

        [FromForm]
        public required string PhoneNumber { get; set; }

        [FromForm]
        public required string Email { get; set; }

        [FromForm]
        public required string Region { get; set; }

        [FromForm]
        public required string City { get; set; }

        [FromForm]
        public required string PostalCode { get; set; }

        [FromForm]
        public required string StreetName { get; set; }

        [FromForm]
        public required string HouseNumber { get; set; }

        [FromForm]
        public string? ApartmentNumber { get; set; }

    }

}
