using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Models.Users;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
   Task<IEnumerable<Log>> GetAllLogs(CancellationToken token);
   Task<IEnumerable<Log>> GetAllLogsPerUser(int userID);
    Task<bool> AddLog(AddLogRequest request, CancellationToken token);
}
