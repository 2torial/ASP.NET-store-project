namespace ASP.NET_store_project.Server.Utilities
{
    public static class MultipleRequestsAsJsonEndpoint<T>
    {
        public static async Task<IEnumerable<KeyValuePair<U, T?>>> GetAsync<U>(IDictionary<U, ClientData> clientsData, string? requestAdress = null)
        {
            if (requestAdress == null && clientsData.Any(kvp => kvp.Value.RequestAdress == null))
                throw new ArgumentNullException(nameof(requestAdress));

            var messages = await Task.WhenAll(clientsData
                .Select(async kvp => new KeyValuePair<U, HttpResponseMessage>(
                    kvp.Key,
                    await kvp.Value.Client.GetAsync(requestAdress ?? kvp.Value.RequestAdress))));

            return await Resolve(messages);
        }

        public static async Task<IEnumerable<KeyValuePair<U, T?>>> SendAsync<U>(IDictionary<U, ClientData> clientsData, string? requestAdress = null, HttpContent? content = null) {
            if (requestAdress == null && clientsData.Any(kvp => kvp.Value.RequestAdress == null))
                throw new ArgumentNullException(nameof(requestAdress));
            if (content == null && clientsData.Any(kvp => kvp.Value.Content == null))
                throw new ArgumentNullException(nameof(content));

            var messages = await Task.WhenAll(clientsData
                .Select(async kvp => new KeyValuePair<U, HttpResponseMessage>(
                    kvp.Key,
                    await kvp.Value.Client.PostAsync(
                        requestAdress ?? kvp.Value.RequestAdress,
                        content ?? kvp.Value.Content))));

            return await Resolve(messages);
        }

        private static async Task<IEnumerable<KeyValuePair<U, T?>>> Resolve<U>(IEnumerable<KeyValuePair<U,HttpResponseMessage>> messages)
        {
            var succeededMessages = messages.Where(kvp => kvp.Value.IsSuccessStatusCode);

            return await Task.WhenAll(succeededMessages
                .Select(async kvp => new KeyValuePair<U, T?>(kvp.Key, await kvp.Value.Content.ReadFromJsonAsync<T>())));
        }

    }

    public class ClientData(HttpClient client)
    {
        public HttpClient Client { get; } = client;
        public string? RequestAdress { get; set; }
        public HttpContent? Content { get; set; }
    }
}
