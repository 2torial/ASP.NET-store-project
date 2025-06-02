namespace ASP.NET_store_project.Server.Models.StructuredData
{
    // Uniform class for server-server and server-client communication
    public record CustomerInfo(string Name, string Surname, string PhoneNumber, string Email);

}
