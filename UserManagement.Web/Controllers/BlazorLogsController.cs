using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Services.Logs.Models;
using UserManagement.Services.Logs.Services;

namespace UserManagement.WebMS.Controllers;

[Route("blazor/logs")]
public class BlazorLogsController : Controller
{
    private readonly ILogService _logService;
    public BlazorLogsController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpPost("List")]
    public async Task<IActionResult> List([FromBody] LogListViewModel viewModel, CancellationToken cancellationToken)
    {
        LogListViewModel model = await CreateLogsListViewModel(viewModel,cancellationToken);
        return Ok(model);
    }

    private async Task<LogListViewModel> CreateLogsListViewModel(LogListViewModel viewModel,CancellationToken token)
    {
        var logs = await _logService.GetLogs(viewModel,token);

        var model = viewModel;
        model.Logs  = logs.Select(l => (LogDTO)l).ToList();
        return model;
    }
}
