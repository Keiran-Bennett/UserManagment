using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    Task<bool> AddUser(User user);
    Task<bool> EditUser(User user);
    Task<User?> GetUser(long userID);
    Task<IEnumerable<User>> GetActiveUsers();
    Task<IEnumerable<User>> GetInActiveUsers();
    Task<IEnumerable<User>> GetAll();
    Task<bool> DeleteUser(long userID);
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
