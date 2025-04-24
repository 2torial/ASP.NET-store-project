namespace ASP.NET_store_project.Server.Utilities.MultipleRequests
{
    public static class MultipleRequestsEndpoint
    {
        public static async Task<KeyValuePair<U, T?>[]> GetAsync<U, T>(IDictionary<U, ClientData> clientsData, Func<HttpResponseMessage, Task<T?>>? resolve = null)
        {
            if (clientsData.Any(kvp => kvp.Value.RequestAdress == null))
                throw new NullReferenceException("Found empty RequestAdress!");

            var messages = await Task.WhenAll(clientsData
                .Select(async kvp => new KeyValuePair<U, HttpResponseMessage>(
                    kvp.Key,
                    await kvp.Value.Client.GetAsync(kvp.Value.RequestAdress))));

            return resolve == null
                ? await Task.WhenAll(messages.Where(kvp => kvp.Value.IsSuccessStatusCode)
                    .Select(async kvp => new KeyValuePair<U, T?>(kvp.Key, await kvp.Value.Content.ReadFromJsonAsync<T>())))
                : await Task.WhenAll(messages
                    .Select(async kvp => new KeyValuePair<U, T?>(kvp.Key, await resolve.Invoke(kvp.Value))));
        }

        public static async Task<KeyValuePair<U, T?>[]> SendAsync<U, T>(IDictionary<U, ClientData> clientsData, Func<HttpResponseMessage, Task<T?>>? resolve = null)
        {
            if (clientsData.Any(kvp => kvp.Value.RequestAdress == null))
                throw new NullReferenceException("Found empty RequestAdress!");
            if (clientsData.Any(kvp => kvp.Value.Content == null))
                throw new NullReferenceException("Found empty Content!");

            var messages = await Task.WhenAll(clientsData
                .Select(async kvp => new KeyValuePair<U, HttpResponseMessage>(
                    kvp.Key,
                    await kvp.Value.Client.PostAsync(
                        kvp.Value.RequestAdress,
                        kvp.Value.Content))));

            return resolve == null
                ? await Task.WhenAll(messages.Where(kvp => kvp.Value.IsSuccessStatusCode)
                    .Select(async kvp => new KeyValuePair<U, T?>(kvp.Key, await kvp.Value.Content.ReadFromJsonAsync<T>())))
                : await Task.WhenAll(messages
                    .Select(async kvp => new KeyValuePair<U, T?>(kvp.Key, await resolve.Invoke(kvp.Value))));
        }

    }
}
