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
            var logsQuery = GetLogsQuery(viewModel);
            return await logsQuery.ToListAsync(cancellationToken);
        }
        catch
        {
            return new List<Log>();
        }
    }

    private IQueryable<Log> GetLogsQuery(LogListViewModel viewModel)
    {
        var logsQuery = GetLogsWithinDatePeriod(viewModel);
        logsQuery = ApplySpecificFilters(viewModel, logsQuery);
        return logsQuery;
    }

    private static IQueryable<Log> ApplySpecificFilters(LogListViewModel viewModel, IQueryable<Log> logsQuery)
    {
        if (viewModel.IsFilterEnabled)
        {
            if ((int)viewModel.Type > 0)
                logsQuery = logsQuery.Where(l => l.Type == viewModel.Type);
            if (!string.IsNullOrEmpty(viewModel.SearchTerm))
                logsQuery = logsQuery.Where(l => l.Details.Contains(viewModel.SearchTerm));
        }

        return logsQuery;
    }

    private IQueryable<Log> GetLogsWithinDatePeriod(LogListViewModel viewModel) => _dataAccess.GetAll<Log>()
                   .Where(l => DateOnly.FromDateTime(l.DateofAction) >= viewModel.StartDate & DateOnly.FromDateTime(l.DateofAction) <= viewModel.EndDate).AsQueryable();

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
