using System.Collections.Generic;

namespace UserManagement.Web.Models.Users;

public class LogListViewModel
{
    public List<LogDTO> Logs { get; set; } = new();
}
