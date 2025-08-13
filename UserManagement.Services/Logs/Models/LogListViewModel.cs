using System;
using System.Collections.Generic;

namespace UserManagement.Services.Logs.Models;

public class LogListViewModel
{
    public LogDTO SelectedLog { get; set; } = new();
    public List<LogDTO> Logs { get; set; } = new();
    public string SearchTerm { get; set; } = default!;
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1));
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public int UserID { get; set; }
    public int Type { get; set; }
    public bool IsFilterEnabled { get; set; }   
}
