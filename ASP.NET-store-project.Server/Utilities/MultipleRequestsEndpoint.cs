namespace ASP.NET_store_project.Server.Utilities
{
    public static class MultipleRequestsEndpoint<T>
    {
        public static async Task<V?[]> GetAsync<U, V>(
            IEnumerable<U> requestBases,
            Func<U, GetRequestOptions> configureRequest,
            Func<HttpResponseMessage, Task<T?>> resolveMessage,
            Func<U, T?, V> modifyResult) => await Task.WhenAll(requestBases
                .Select(async endpoint => {
                    var settings = configureRequest.Invoke(endpoint);
                    var message = await settings.HttpClient.GetAsync(settings.RequestAdress);
                    var result =  await resolveMessage.Invoke(message);
                    return modifyResult.Invoke(endpoint, result);
                }));

        public static async Task<V?[]> GetAsync<U, V>(
            IEnumerable<U> requestBases,
            Func<U, GetRequestOptions> configureRequest,
            Func<U, T?, V> modifyResult) =>
                await GetAsync(
                    requestBases,
                    configureRequest,
                    async msg => msg.IsSuccessStatusCode 
                        ? await msg.Content.ReadFromJsonAsync<T>() 
                        : await Task.FromResult(default(T)), 
                    modifyResult);

        public static async Task<T?[]> GetAsync<U>(
            IEnumerable<U> requestBases,
            Func<U, GetRequestOptions> configureRequest,
            Func<HttpResponseMessage, Task<T?>> resolveMessage) =>
                await GetAsync(
                    requestBases,
                    configureRequest, 
                    resolveMessage, 
                    (_, res) => res);

        public static async Task<T?[]> GetAsync<U>(
            IEnumerable<U> requestBases,
            Func<U, GetRequestOptions> configureRequest) =>
                await GetAsync(
                    requestBases,
                    configureRequest, 
                    async msg => msg.IsSuccessStatusCode
                        ? await msg.Content.ReadFromJsonAsync<T>()
                        : await Task.FromResult(default(T)), 
                    (_, res) => res);

        public static async Task<V?[]> PostAsync<U, V>(
            IEnumerable<U> requestBases,
            Func<U, PostRequestOptions> configureRequest,
            Func<HttpResponseMessage, Task<T?>> resolveMessage,
            Func<U, T?, V> modifyResult) => await Task.WhenAll(requestBases
                .Select(async endpoint => {
                    var settings = configureRequest.Invoke(endpoint);
                    var message = await settings.HttpClient.PostAsync(settings.RequestAdress, settings.Content);
                    var result = await resolveMessage.Invoke(message);
                    return modifyResult.Invoke(endpoint, result);
                }));

        public static async Task<V?[]> PostAsync<U, V>(
            IEnumerable<U> requestBases,
            Func<U, PostRequestOptions> configureRequest,
            Func<U, T?, V> modifyResult) =>
                await PostAsync(
                    requestBases,
                    configureRequest,
                    async msg => msg.IsSuccessStatusCode
                        ? await msg.Content.ReadFromJsonAsync<T>()
                        : await Task.FromResult(default(T)),
                    modifyResult);

        public static async Task<T?[]> PostAsync<U>(
            IEnumerable<U> requestBases,
            Func<U, PostRequestOptions> configureRequest,
            Func<HttpResponseMessage, Task<T?>> resolveMessage) =>
                await PostAsync(
                    requestBases,
                    configureRequest,
                    resolveMessage,
                    (_, res) => res);

        public static async Task<T?[]> PostAsync<U>(
            IEnumerable<U> requestBases,
            Func<U, PostRequestOptions> configureRequest) =>
                await PostAsync(
                    requestBases,
                    configureRequest,
                    async msg => msg.IsSuccessStatusCode
                        ? await msg.Content.ReadFromJsonAsync<T>()
                        : await Task.FromResult(default(T)),
                    (_, res) => res);

        public record GetRequestOptions(HttpClient HttpClient, string RequestAdress);

        public record PostRequestOptions(HttpClient HttpClient, string RequestAdress, HttpContent Content);

    }
}
