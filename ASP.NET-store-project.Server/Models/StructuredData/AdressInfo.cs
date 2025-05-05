using System.Text.Json.Serialization;

namespace ASP.NET_store_project.Server.Models.StructuredData
{
    [method: JsonConstructor]
    public record AdressInfo(
        string Region, 
        string City, 
        string PostalCode, 
        string StreetName, 
        string HouseNumber, 
        string? ApartmentNumber);
}
