using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    Task<bool> AddUser(User user, CancellationToken cancellationToken);
    Task<bool> EditUser(User user, CancellationToken cancellationToken);
    Task<User> GetUser(int userID, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetActiveUsers(CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetInActiveUsers(CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken);
    Task<bool> DeleteUser(int userID, CancellationToken cancellationToken);
}

public interface ILogService
{
   Task<IEnumerable<Log>> GetAllLogs();
   Task<IEnumerable<Log>> GetAllLogsPerUser(int userID);
    Task<bool> AddLog(AddLogRequest request);
}

public class AddLogRequest
{
   public long UserID { get; set; }
   public DateTime DateOfAction { get; set; }
   public string Details { get; set; } = string.Empty;
}
