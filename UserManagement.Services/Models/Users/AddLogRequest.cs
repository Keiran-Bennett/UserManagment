using System;

namespace UserManagement.Services.Models.Users;

public class AddLogRequest
{
   public long UserID { get; set; }
   public DateTime DateOfAction { get; set; }
   public string Details { get; set; } = string.Empty;
    public LogType logType { get; set; }
}
