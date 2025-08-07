using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    User? GetUser(long userID);
    IEnumerable<User> GetActiveUsers();
    IEnumerable<User> GetInActiveUsers();
    IEnumerable<User> GetAll();
}
