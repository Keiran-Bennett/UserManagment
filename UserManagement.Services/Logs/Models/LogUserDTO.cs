using System;
using UserManagement.Models;

namespace UserManagement.Services.Logs.Models;

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
