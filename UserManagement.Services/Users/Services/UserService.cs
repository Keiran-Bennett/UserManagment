using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Logs.Models;
using UserManagement.Services.Logs.Services;

namespace UserManagement.Services.Users.Services;

public class UserService : IUserService
{
    private readonly IDataContext _dataContext;
    private readonly ILogService _logService;
    public UserService(IDataContext dataContext, ILogService logService)
    {
        _dataContext = dataContext;
        _logService = logService;
    }
    public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            return await _dataContext.GetAll<User>().ToListAsync(cancellationToken);
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
            return await _dataContext.GetAll<User>().Where(u => u.IsActive).ToListAsync(cancellationToken);
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
            return await _dataContext.GetAll<User>().Where(u => !u.IsActive).ToListAsync(cancellationToken);
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
        var user = await _dataContext.Get<User>(u => u.Id == userID,cancellationToken);
        return user;
    }

    public async Task<bool> EditUser(User user, CancellationToken cancellationToken)
    {
        try
        {
            await _dataContext.Update(user, cancellationToken);
            var editUserLogRequest = CreateAddLogRequest(user);
            await LogAction(editUserLogRequest, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static CreateLogRequest CreateAddLogRequest(User user) => new CreateLogRequest
    {   UserID = user.Id,
        DateOfAction = System.DateTime.Now,
        Details = $"{user.Forename + " " + user.Surname} has been updated",
        logType = LogType.Update,
        JSONSnapShot = JsonSerializer.Serialize((LogUserDTO)user)
    };

    public async Task<bool> DeleteUser(int userID, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Get<User>(u => u.Id == userID, cancellationToken);
        if (user is null)
            return false;

        await _dataContext.Delete(user, cancellationToken);
        var logDeleteRequest = CreateAddDeleteLogRequest(userID, user);
        await LogAction(logDeleteRequest, cancellationToken);
        return true;
    }

    private static CreateLogRequest CreateAddDeleteLogRequest(int userID, User user) => new CreateLogRequest { UserID = userID, DateOfAction = System.DateTime.Now, Details = $"{user.Forename + " " + user.Surname} has been deleted", logType = LogType.Delete, JSONSnapShot = JsonSerializer.Serialize((LogUserDTO)user) };

    public async Task<bool> AddUser(User user, CancellationToken cancellationToken)
    {
        try
        {
            await _dataContext.Create(user, cancellationToken);
            var logAddNewUserRequest = new CreateLogRequest { UserID = user.Id, DateOfAction = System.DateTime.Now, Details = $"{user.Forename + " " + user.Surname} has been added", logType = LogType.Add, JSONSnapShot = JsonSerializer.Serialize((LogUserDTO)user) };
            await LogAction(logAddNewUserRequest, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
       
    }

    private async Task LogAction(CreateLogRequest logRequest, CancellationToken cancellationToken) => await _logService.AddLog(logRequest,cancellationToken);
}

