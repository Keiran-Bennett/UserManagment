using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using UserManagement.Models;
using UserManagement.Services;
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
    public async Task<ViewResult> List()
    {
        LogListViewModel viewModel = await CreateLogsListViewModel();
        return View(viewModel);
    }

    private async Task<LogListViewModel> CreateLogsListViewModel()
    {
        var logs = await _logService.GetAllLogs();
        var model = new LogListViewModel() { Logs = logs.Select(l => (LogDTO)l).ToList() };
        return model;
    }
}
