using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public IEnumerable<User> GetActiveUsers() => _dataAccess.GetAll<User>().Where(u => u.IsActive);
    public IEnumerable<User> GetInActiveUsers() => _dataAccess.GetAll<User>().Where(u => !u.IsActive);
    public User? GetUser(long userID) => _dataAccess.Get<User>(u => u.Id == userID);
}
