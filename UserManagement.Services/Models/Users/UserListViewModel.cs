using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Web.Models.Users;

public class UserInfoViewModel
{
    public long Id { get; set; }
    public string Forename { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public bool IsActive { get; set; }


}

public class UserListViewModel
{
    public List<UserListItemViewModel> Users { get; set; } = new();
}

public class LogListViewModel
{
    public List<LogItemViewModel> Logs { get; set; } = new();
}


public class UserEditViewModel
{
    public bool IsSuccess { get; set; }
    public UserListItemViewModel User { get; set; } = new();
}

public class UserAddViewModel
{
    public bool IsSuccess { get; set; }
    public UserListItemViewModel User { get; set; } = new();
}

public class UserDeleteViewModel
{
    public bool IsSuccess { get; set; }
    public UserListItemViewModel User { get; set; } = new();
}

public class LogItemViewModel
{
    public long Id { get; set; }
    public int UserID { get; set; } = default!;
    public DateTime DateofAction { get; set; } = default!;
    public string Details { get; set; } = default!;

    public static explicit operator Log(LogItemViewModel model) => new Log
    {
        Id = (int) model.Id,
        UserID = model.UserID,
        DateofAction = model.DateofAction,
        Details = model.Details,
    };

    public static explicit operator LogItemViewModel(Log log) => new LogItemViewModel
    {
        Id = log.Id,
        UserID = log.UserID,
        DateofAction = log.DateofAction,
        Details = log.Details,
    };

}


public class UserListItemViewModel
{
    public int Id { get; set; }
    public string Forename { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth {  get; set; }
    public bool IsActive { get; set; }
    public List<LogItemViewModel> Logs { get; set; } = new();

    public static explicit operator UserListItemViewModel(User user) => new UserListItemViewModel
    {
        Id = user.Id,
        Forename = user.Forename,
        Surname = user.Surname,
        Email = user.Email,
        DateOfBirth = user.DateOfBirth,
        IsActive = user.IsActive,
        Logs = user.Logs.Select(l => (LogItemViewModel) l).ToList()
    };

    public static explicit operator User(UserListItemViewModel model) => new User
    {
        Id = (int) model.Id,
        Forename = model.Forename,
        Surname = model.Surname,
        Email = model.Email,
        DateOfBirth = model.DateOfBirth,
        IsActive = model.IsActive,
        Logs = model.Logs.Select(l => (Log)l).ToList()
    };
}
