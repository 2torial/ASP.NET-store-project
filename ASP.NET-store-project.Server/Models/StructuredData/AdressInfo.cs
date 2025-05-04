namespace ASP.NET_store_project.Server.Models.StructuredData
{
    public record AdressInfo(
        string Region, 
        string City, 
        string PostalCode, 
        string StreetName, 
        string HouseNumber, 
        string? ApartmentNumber);
}
