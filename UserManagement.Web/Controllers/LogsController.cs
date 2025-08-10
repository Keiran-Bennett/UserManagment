using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public async Task<ViewResult> List(LogListViewModel viewmodel, CancellationToken cancellationToken)
    {
        LogListViewModel viewModel = await CreateLogsListViewModel(cancellationToken);
        return View(viewModel);
    }

    private async Task<LogListViewModel> CreateLogsListViewModel(CancellationToken token)
    {
        var logs = await _logService.GetAllLogs(token);
        var model = new LogListViewModel() { Logs = logs.Select(l => (LogDTO)l).ToList() };
        return model;
    }
}
