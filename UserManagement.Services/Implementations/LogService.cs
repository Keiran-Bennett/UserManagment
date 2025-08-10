using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Models.Users;

namespace UserManagement.Services.Domain.Implementations;

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public async Task<bool> AddLog(AddLogRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(1);
        return true;
    }

    public async Task<IEnumerable<Log>> GetAllLogs(CancellationToken token)
    {
        await Task.Delay(1);
        return new List<Log>();
    }

    public async Task<IEnumerable<Log>> GetAllLogsPerUser(int userID)
    {
        await Task.Delay(1);
        return new List<Log>();
    }
}
