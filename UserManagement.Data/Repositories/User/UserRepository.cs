using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;


namespace UserManagement;
public class UserRepository : IUserRepository
{
    private DataContext _context;

    public UserRepository(DataContext context) => _context = context;

    public async Task<User?> GetUsersWithLogs(int userID, CancellationToken token)
    {
        try
        {
            return await _context.Users
                .Where(u => u.Id == userID)
                .Include(x => x.Logs)
                .FirstOrDefaultAsync(token);
        }
        catch
        {
            throw;
        }
    }

}
