using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Logs;

public interface ILogService
{
   Task<IEnumerable<Log>> GetLogs( LogListViewModel logListViewModel,CancellationToken cancellationToken);
   Task<IEnumerable<Log>> GetAllLogsPerUser(int userID, CancellationToken cancellationToken);
    Task<bool> AddLog(AddLogRequest request, CancellationToken cancellationToken);
}
