using System;

namespace UserManagement.Services.Logs.Models;

public class CreateLogRequest
{
    public int UserID { get; set; }
    public DateTime DateOfAction { get; set; }
    public string Details { get; set; } = string.Empty;
    public LogType logType { get; set; }
    public string JSONSnapShot { get; set; } = default!;
}
