namespace UserManagment.Blazor.APIService;

using System.Net.Http.Json;

public class ApiService : IApiService
{
    private readonly HttpClient _http;

    public ApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiClient");
    }

    public async Task<T?> GetJsonAsync<T>(string endpoint)
    {
        return await _http.GetFromJsonAsync<T>(endpoint);
    }

    public async Task<TResponse?> PostJsonAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var response = await _http.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<string> PostFormAsync(string endpoint, Dictionary<string, string> formFields)
    {
        var content = new FormUrlEncodedContent(formFields);
        var response = await _http.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    // POST raw string content (e.g., XML, plain text)
    public async Task<string> PostRawAsync(string endpoint, string content, string mediaType = "text/plain")
    {
        var stringContent = new StringContent(content, System.Text.Encoding.UTF8, mediaType);
        var response = await _http.PostAsync(endpoint, stringContent);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}

