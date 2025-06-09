namespace ASP.NET_store_project.Server.Models.StructuredData;

// Uniform class for server-server and server-client communication
public record OrderStageInfo(string Type, string DateOfCreation, string TimeStamp);
