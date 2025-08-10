using System;
using UserManagement.Models;

namespace UserManagement.Services.Logs;

public class LogDTO
{
    public long Id { get; set; }
    public int UserID { get; set; } = default!;
    public DateTime DateofAction { get; set; } = default!;
    public string Details { get; set; } = default!;
    public LogType logType { get; set; }


    public static explicit operator Log(LogDTO model) => new Log
    {
        Id = (int) model.Id,
        UserID = model.UserID,
        DateofAction = model.DateofAction,
        Details = model.Details,
        Type = (int) model.logType,
    };

    public static explicit operator LogDTO(Log log) => new LogDTO
    {
        Id = log.Id,
        UserID = log.UserID,
        DateofAction = log.DateofAction,
        Details = log.Details,
        logType = (LogType) log.Type
    };

}
