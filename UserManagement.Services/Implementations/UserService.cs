using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Models.Users;

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
    public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            return await _dataAccess.GetAll<User>().ToListAsync(cancellationToken);
        }
        catch
        {
            return new List<User>();
        }
    }


    public async Task<IEnumerable<User>> GetActiveUsers(CancellationToken cancellationToken)
    {
        try
        {
            return await _dataAccess.GetAll<User>().Where(u => u.IsActive).ToListAsync(cancellationToken);
        }
        catch
        {
            return new List<User>();
        }
    }

    public async Task<IEnumerable<User>> GetInActiveUsers(CancellationToken cancellationToken)
    {
        try
        {
            return await _dataAccess.GetAll<User>().Where(u => !u.IsActive).ToListAsync(cancellationToken);
        }
        catch
        {
            return new List<User>();
        }
    }

    public async Task<User> GetUser(int userID, CancellationToken cancellationToken)
    {
        try
        {
            return await GetUserWithLogs(userID, cancellationToken) ?? throw new System.Exception("User cannot be found");
        }
        catch
        {
            throw;
        }        
    }
    
    private async Task<User?> GetUserWithLogs(int userID, CancellationToken cancellationToken)
    {
        var user = await _dataAccess.Get<User>(u => u.Id == userID,cancellationToken);
        if (user != null)
        {
            var logs = await _logService.GetAllLogsPerUser(user.Id,cancellationToken);
            user.Logs = logs.ToList();
        }

        return user;
    }

    public async Task<bool> EditUser(User user, CancellationToken cancellationToken)
    {
        try
        {
            await _dataAccess.Update(user, cancellationToken);
            var editUserLogRequest = new AddLogRequest { UserID = user.Id, DateOfAction = System.DateTime.Now, Details = $"{user.Forename + " " + user.Surname} has been updated to {JsonSerializer.Serialize(user)}", logType = LogType.Update };
            await LogAction(editUserLogRequest, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteUser(int userID, CancellationToken cancellationToken)
    {
        User? user = await _dataAccess.Get<User>(u => u.Id == userID, cancellationToken);
        if (user is null)
            return false;

        await _dataAccess.Delete(user, cancellationToken);
        var logDeleteRequest = new AddLogRequest { UserID = userID, DateOfAction = System.DateTime.Now, Details = $"{user.Forename + " " + user.Surname} has been deleted", logType = LogType.Delete};
        await LogAction(logDeleteRequest, cancellationToken);
        return true;
    }

    public async Task<bool> AddUser(User user, CancellationToken cancellationToken)
    {
        try
        {
            await _dataAccess.Create(user, cancellationToken);
            var logAddNewUserRequest = new AddLogRequest { UserID = user.Id, DateOfAction = System.DateTime.Now, Details = $"{user.Forename + " " + user.Surname} has been added with {JsonSerializer.Serialize(user)}", logType = LogType.Add };
            await LogAction(logAddNewUserRequest, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
       
    }

    private async Task LogAction(AddLogRequest logRequest, CancellationToken cancellationToken) => await _logService.AddLog(logRequest,cancellationToken);
}

