namespace ASP.NET_store_project.Server.Utilities
{
    public static class MultipleRequestsAsJsonEndpoint<T>
    {
        public static async Task<KeyValuePair<Guid, T?>[]> SendAsync(Dictionary<Guid, HttpClient> clients, string requestAdress, HttpContent content) {
            var messages = await Task.WhenAll(clients
                .Select(async client => new KeyValuePair<Guid, HttpResponseMessage>(
                    client.Key, await client.Value.PostAsync(requestAdress, content))));

            return await ResolveResponse(messages);
        }

        public static async Task<KeyValuePair<Guid, T?>[]> SendAsync(Dictionary<Guid, (HttpClient, StringContent)> clients, string requestAdress)
        {
            var messages = await Task.WhenAll(clients
                .Select(async client => new KeyValuePair<Guid, HttpResponseMessage>(
                    client.Key, await client.Value.Item1.PostAsync(requestAdress, client.Value.Item2))));

            return await ResolveResponse(messages);
        }

        private static async Task<KeyValuePair<Guid, T?>[]> ResolveResponse(KeyValuePair<Guid, HttpResponseMessage>[] messages) =>
            await Task.WhenAll(messages.Select(async msg => new KeyValuePair<Guid, T?>(msg.Key, await msg.Value.Content.ReadFromJsonAsync<T>())));
    }
}
