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
}

