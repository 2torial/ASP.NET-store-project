namespace ASP.NET_store_project.Server.Models.StructuredData;

using System.Text.Json.Serialization;

// Uniform class for server-server and server-client communication
[method: JsonConstructor]
public record AdressInfo(
    string Region, 
    string City, 
    string PostalCode, 
    string StreetName, 
    string HouseNumber, 
    string? ApartmentNumber);
