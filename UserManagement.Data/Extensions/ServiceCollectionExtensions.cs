using Microsoft.EntityFrameworkCore;
using UserManagement.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
        => services.AddDbContext<IDataContext, DataContext>(options => options.UseSqlServer("Server=LAPTOP-I8KQ8AMQ;Database=Inflo;Integrated Security=True;Trust Server Certificate=True;") );
}
