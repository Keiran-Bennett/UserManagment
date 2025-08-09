
namespace UserManagment.Blazor.APIService;

public interface IApiService
{
    Task<T?> GetJsonAsync<T>(string endpoint);
    Task<string> PostFormAsync(string endpoint, Dictionary<string, string> formFields);
    Task<TResponse?> PostJsonAsync<TRequest, TResponse>(string endpoint, TRequest data);
    Task<string> PostRawAsync(string endpoint, string content, string mediaType = "text/plain");
}
