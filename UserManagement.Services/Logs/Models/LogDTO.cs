using System;
using System.Text.Json;
using UserManagement.Models;

namespace UserManagement.Services.Logs.Models;

public class LogDTO
{
    public int Id { get; set; }
    public int UserID { get; set; } = default!;
    public DateTime DateofAction { get; set; } = default!;
    public string Details { get; set; } = default!;
    public LogType LogType { get; set; }
    public LogUserDTO Snapshot { get; set; } = new();


    public static explicit operator Log(LogDTO model) => new Log
    {
        Id = (int) model.Id,
        UserID = model.UserID,
        DateofAction = model.DateofAction,
        Details = model.Details,
        Type = (int) model.LogType,
        SnapShot = JsonSerializer.Serialize(model.Snapshot),
    };

    public static explicit operator LogDTO(Log log) => new LogDTO
    {
        Id = log.Id,
        UserID = log.UserID,
        DateofAction = log.DateofAction,
        Details = log.Details,
        LogType = (LogType) log.Type,
        Snapshot = JsonSerializer.Deserialize<LogUserDTO>(log.SnapShot) ?? new LogUserDTO { },
    };
}
