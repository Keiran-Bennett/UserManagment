using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Logs;

namespace UserManagement.Services.Users.Models;

public class UserDTO
{
    public int Id { get; set; }
    public string Forename { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth {  get; set; }
    public bool IsActive { get; set; }
    public List<LogDTO> Logs { get; set; } = new();

    public static explicit operator UserDTO(User user) => new UserDTO
    {
        Id = user.Id,
        Forename = user.Forename,
        Surname = user.Surname,
        Email = user.Email,
        DateOfBirth = user.DateOfBirth,
        IsActive = user.IsActive,
        Logs = user.Logs.Select(l => (LogDTO) l).ToList()
    };

    public static explicit operator User(UserDTO model) => new User
    {
        Id =  model.Id,
        Forename = model.Forename,
        Surname = model.Surname,
        Email = model.Email,
        DateOfBirth = model.DateOfBirth,
        IsActive = model.IsActive,
        Logs = model.Logs.Select(l => (Log)l).ToList()
    };
}
