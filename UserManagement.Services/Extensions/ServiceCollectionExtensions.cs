using UserManagement.Services.Logs;
using UserManagement.Services.Users.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILogService, LogService>();
        return services;
    } 
}
