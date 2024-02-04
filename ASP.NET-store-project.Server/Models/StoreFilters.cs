namespace ASP.NET_store_project.Server.Models
{
    public class StoreFilters
    {
        public ValueRange PriceRange { get; set; }

        public List<PossibleConfiguration> Configurations { get; set; }

        public class ValueRange
        {
            public int From { get; set; }

            public int To { get; set; }
        }

        public class PossibleConfiguration
        {
            public string Label { get; set; }

            public List<string> Parameters { get; set; }
        }
    }
}
