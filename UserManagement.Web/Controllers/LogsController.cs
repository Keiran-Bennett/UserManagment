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
    public async Task<ViewResult> List(LogListViewModel viewModel, CancellationToken cancellationToken)
    {
        LogListViewModel model = await CreateLogsListViewModel(viewModel,cancellationToken);
        return View(model);
    }

    private async Task<LogListViewModel> CreateLogsListViewModel(LogListViewModel viewModel,CancellationToken token)
    {
        var logs = await _logService.GetLogs(viewModel,token);
        var model = new LogListViewModel() { Logs = logs.Select(l => (LogDTO)l).ToList() };
        return model;
    }
}
