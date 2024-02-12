namespace ASP.NET_store_project.Server.Models
{
    public class OrderListComponentData
    {
        public List<OrderData> Orders { get; set; }

        public class OrderData
        {
            public int OrderId { get; set; }

            public UserData CustomerDetails { get; set; }

            public AdressData AdressDetails { get; set; }

            public string? CurrentStatus { get; set; }

            public class UserData
            {
                public string CustomerId { get; set; }

                public string Name { get; set; }

                public string Surname { get; set; }

                public string PhoneNumber { get; set; }

                public string Email { get; set; }
            }

            public class AdressData
            {
                public string Region { get; set; }

                public string City { get; set; }

                public string PostalCode { get; set; }

                public string StreetName { get; set; }

                public string HouseNumber { get; set; }

                public string? ApartmentNumber { get; set; }
            }
        }
    }
}
