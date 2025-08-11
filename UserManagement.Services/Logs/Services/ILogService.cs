using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Logs.Models;

namespace UserManagement.Services.Logs.Services;

public interface ILogService
{
   Task<IEnumerable<Log>> GetLogs( LogListViewModel logListViewModel,CancellationToken cancellationToken);
   Task<IEnumerable<Log>> GetAllLogsPerUser(int userID, CancellationToken cancellationToken);
    Task<bool> AddLog(AddLogRequest request, CancellationToken cancellationToken);
}
