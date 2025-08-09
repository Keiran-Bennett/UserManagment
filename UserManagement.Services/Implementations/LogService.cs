using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

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
