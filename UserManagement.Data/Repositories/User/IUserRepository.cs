using System.Threading;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement;
public interface IUserRepository
{
    Task<User?> GetUsersWithLogs(int userID, CancellationToken token);
}
