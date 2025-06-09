namespace ASP.NET_store_project.Server.Utilities;

// Offers an uniform way of sending multiple async requests to individual external APIs
// Implements GET and POST request methods
public static class MultipleRequestsEndpoint<T>
{
    // Main method for GET requests
    // requestBases: iterable-type on which endpoints are built
    // configureRequest(requestBase): maps an individual requestBase to HttpClient with individual configuration
    // resolveMessage(responseMessage): resolves an individual response (for each of the requests)
    // modifyResult(originalRequestBase, resolvedResponse): further modifies the request results having access to original requestBase
    public static async Task<V?[]> GetAsync<U, V>(
        IEnumerable<U> requestBases,
        Func<U, GetRequestOptions> configureRequest,
        Func<HttpResponseMessage, Task<T?>> resolveMessage,
        Func<U, T?, V> modifyResult) => await Task.WhenAll(requestBases                     // all requests are resolved asynchronously
            .Select(async endpoint => {                                                     // maps every requestBase
                var settings = configureRequest.Invoke(endpoint);                           // retrieves client config
                var message = await settings.HttpClient.GetAsync(settings.RequestAdress);   // sends request
                var result =  await resolveMessage.Invoke(message);                         // resolves response message
                return modifyResult.Invoke(endpoint, result);                               // returns modified result
            }));

    // Ignores failed requests and assumes default value as an response result
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

    // Doesn't modify response result
    public static async Task<T?[]> GetAsync<U>(
        IEnumerable<U> requestBases,
        Func<U, GetRequestOptions> configureRequest,
        Func<HttpResponseMessage, Task<T?>> resolveMessage) =>
            await GetAsync(
                requestBases,
                configureRequest, 
                resolveMessage, 
                (_, res) => res);

    // Ignores failed requests (assumes default value) and doesn't modify results
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

    // Main method for POST requests
    // It works practically the same as the GET one
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

    // As above
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

    // As above
    public static async Task<T?[]> PostAsync<U>(
        IEnumerable<U> requestBases,
        Func<U, PostRequestOptions> configureRequest,
        Func<HttpResponseMessage, Task<T?>> resolveMessage) =>
            await PostAsync(
                requestBases,
                configureRequest,
                resolveMessage,
                (_, res) => res);

    // As above
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

    // Record types used in the methods above to configure an endpoint

    public record GetRequestOptions(HttpClient HttpClient, string RequestAdress);

    public record PostRequestOptions(HttpClient HttpClient, string RequestAdress, HttpContent Content);

}
