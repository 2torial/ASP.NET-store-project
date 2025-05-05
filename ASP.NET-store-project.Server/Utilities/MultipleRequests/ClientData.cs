namespace ASP.NET_store_project.Server.Utilities.MultipleRequests
{
    public class ClientData(HttpClient client)
    {
        public HttpClient Client { get; } = client;
        public string? RequestAdress { get; set; }
        public HttpContent? Content { get; set; }
    }
}
