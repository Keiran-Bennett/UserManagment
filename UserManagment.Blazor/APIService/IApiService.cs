
namespace UserManagment.Blazor.APIService;

public interface IApiService
{
    Task<T?> GetJsonAsync<T>(string endpoint);
    Task<TResponse?> PostJsonAsync<TRequest, TResponse>(string endpoint, TRequest data);
}
