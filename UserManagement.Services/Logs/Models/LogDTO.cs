using System;
using System.Text.Json;
using UserManagement.Models;

namespace UserManagement.Services.Logs.Models;

public class LogDTO
{
    public long Id { get; set; }
    public int UserID { get; set; } = default!;
    public DateTime DateofAction { get; set; } = default!;
    public string Details { get; set; } = default!;
    public LogType LogType { get; set; }
    public string UserName { get; set; } = default!;
    public LogUserDTO Snapshot { get; set; } = default!;


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

public class LogUserDTO
{
    public string Forename { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public bool IsActive { get; set; }

    public static explicit operator LogUserDTO(User user) => new LogUserDTO
    {
        Forename = user.Forename,
        Surname = user.Surname,
        Email = user.Email,
        DateOfBirth = user.DateOfBirth,
        IsActive = user.IsActive,
    };

    public static explicit operator User(LogUserDTO model) => new User
    {
        Forename = model.Forename,
        Surname = model.Surname,
        Email = model.Email,
        DateOfBirth = model.DateOfBirth,
        IsActive = model.IsActive,
    };
}
