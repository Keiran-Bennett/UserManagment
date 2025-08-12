using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Logs.Models;

namespace UserManagement.Services.Logs.Services;

public class LogService : ILogService
{
    private readonly IDataContext _dataContext;
    public LogService(IDataContext dataContext) => _dataContext = dataContext;

    public async Task<bool> AddLog(CreateLogRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var log = CreateLog(request);
            await _dataContext.Create(log, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static Log CreateLog(CreateLogRequest request) => new Log
    {
        Details = request.Details,
        DateofAction = request.DateOfAction,
        Type = (int)request.logType,
        UserID = request.UserID,
        SnapShot = request.JSONSnapShot
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
        if (viewModel.IsFilterEnabled)
            logsQuery = ApplySpecificFilters(viewModel, logsQuery);

        return logsQuery;
    }

    private IQueryable<Log> GetLogsWithinDatePeriod(LogListViewModel viewModel) => _dataContext.GetAll<Log>()
        .Where(l => DateOnly.FromDateTime(l.DateofAction) >= viewModel.StartDate & DateOnly.FromDateTime(l.DateofAction) <= viewModel.EndDate).AsQueryable();

    private static IQueryable<Log> ApplySpecificFilters(LogListViewModel viewModel, IQueryable<Log> logsQuery)
    {
        if (viewModel.Type > 0)
            logsQuery = logsQuery.Where(l => l.Type == viewModel.Type);

       if (!string.IsNullOrEmpty(viewModel.SearchTerm))
            logsQuery = logsQuery.Where(l => l.Details.Contains(viewModel.SearchTerm) | (l.SnapShot != null && l.SnapShot.Contains(viewModel.SearchTerm)));

        return logsQuery;
    }
}
