using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    private readonly ILogService _logService;
    public UserService(IDataContext dataAccess, ILogService logService)
    {
        _dataAccess = dataAccess;
        _logService = logService;
    }
    public async Task<IEnumerable<User>> GetAll() => await _dataAccess.GetAll<User>().ToListAsync();
    public async Task<IEnumerable<User>> GetActiveUsers() => await _dataAccess.GetAll<User>().Where(u => u.IsActive).ToListAsync();
    public async Task<IEnumerable<User>> GetInActiveUsers() => await _dataAccess.GetAll<User>().Where(u => !u.IsActive).ToListAsync();
    public async Task<User?> GetUser(long userID)
    {

        var user = await _dataAccess.Get<User>(u => u.Id == userID);
        if (user != null)
        {
            var logs = await _logService.GetAllLogsPerUser(user.Id);
            user.Logs = logs.ToList();
        }

        return user;
    }
    public async Task<bool> EditUser(User user)
    {
        await _dataAccess.Update(user);
        var editUserLogRequest = new AddLogRequest { UserID = user.Id, DateOfAction = System.DateTime.Now, Details = $"{user.Forename + " " + user.Surname} has been updated" };
        await LogAction(editUserLogRequest);

        return true;
    }

    public async Task<bool> DeleteUser(long userID)
    {
        User? user = await _dataAccess.Get<User>(u => u.Id == userID);
        if (user is null)
            return false;

        await _dataAccess.Delete(user);
        var logDeleteRequest = new AddLogRequest { UserID = userID, DateOfAction = System.DateTime.Now, Details = $"{user.Forename + " " + user.Surname} has been deleted" };
        await LogAction(logDeleteRequest);

        return true;
    }

    public async Task<bool> AddUser(User user)
    {
        await _dataAccess.Create(user);
        var logAddNewUserRequest = new AddLogRequest { UserID = user.Id, DateOfAction = System.DateTime.Now, Details = $"{user.Forename + " " + user.Surname} has been added" };
        await LogAction(logAddNewUserRequest);
        return true;
    }

    private async Task LogAction(AddLogRequest logDeleteRequest) => await _logService.AddLog(logDeleteRequest);
}

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public async Task<bool> AddLog(AddLogRequest request)
    {
        Log log = CreateLog(request);
        await _dataAccess.Create(log);
        return true;
    }

    private static Log CreateLog(AddLogRequest request) => new Log { DateofAction = request.DateOfAction, UserID = (int)request.UserID, Details = request.Details };

    public async Task<IEnumerable<Log>> GetAllLogs()
    {
        return await _dataAccess
            .GetAll<Log>()
            .ToListAsync();
    }

    public async Task<IEnumerable<Log>> GetAllLogsPerUser(int userID)
    {
        return await _dataAccess
            .GetAll<Log>()
            .Where(u => u.UserID == userID)
            .ToListAsync();
    }
}
