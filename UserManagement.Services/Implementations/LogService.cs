using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Models.Users;
using UserManagement.Web.Models.Users;

namespace UserManagement.Services.Domain.Implementations;

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public async Task<bool> AddLog(AddLogRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var log = CreateLog(request);
            await _dataAccess.Create<Log>(log, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static Log CreateLog(AddLogRequest request) => new Log
    {
        Details = request.Details,
        DateofAction = request.DateOfAction,
        Type = (int)request.logType,
        UserID = request.UserID
    };

    public async Task<IEnumerable<Log>> GetLogs(LogListViewModel viewModel, CancellationToken cancellationToken)
    {
        try
        {
            var logs = await _dataAccess.GetAll<Log>()
              //  .Where(l => DateOnly.FromDateTime(l.DateofAction) >= viewModel.StartDate & DateOnly.FromDateTime(l.DateofAction) <= viewModel.EndDate)
              // .Where(l => (viewModel.IsFilterEnabled & (int) viewModel.Type > 0) && l.Type == viewModel.Type)
              // .Where(l => (viewModel.IsFilterEnabled & viewModel.SearchTerm != string.Empty) && l.Details.Contains(viewModel.SearchTerm))
               .ToListAsync(cancellationToken);
            return logs;
        }
        catch
        {
            return new List<Log>();
        }
    }

    public async Task<IEnumerable<Log>> GetAllLogsPerUser(int userID, CancellationToken cancellationToken)
    {

        try
        {
            var logs = await _dataAccess.GetAll<Log>().Where(l => l.UserID == userID).ToListAsync(cancellationToken);
            return logs;
        }
        catch
        {
            return new List<Log>();
        }
    }
}
