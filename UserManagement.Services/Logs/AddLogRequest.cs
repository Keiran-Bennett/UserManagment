using System;

namespace UserManagement.Services.Logs;

public class AddLogRequest
{
   public int UserID { get; set; }
   public DateTime DateOfAction { get; set; }
   public string Details { get; set; } = string.Empty;
    public LogType logType { get; set; }
}
