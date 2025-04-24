using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers.BasketController
{
    public class OrderSummaryData
    {
        [FromForm]
        public string? Region { get; init; }

        [FromForm]
        public string? City { get; init; }
        [FromForm]
        public string? StreetName { get; init; }

        [FromForm]
        public string? HouseNumber { get; init; }

        [FromForm]
        public string? ApartmentNumber { get; init; }

        [FromForm]
        public string? Name { get; init; }

        [FromForm]
        public string? Surname { get; init; }

        [FromForm]
        public string? PhoneNumber { get; init; }

        [FromForm]
        public string? Mail { get; init; }

    }
}
