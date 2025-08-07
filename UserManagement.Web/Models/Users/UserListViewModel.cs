using System;

namespace UserManagement.Web.Models.Users;

public class ViewUserListViewModel
{
    public long Id { get; set; }
    public string? Forename { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public bool IsActive { get; set; }
}

public class UserListViewModel
{
    public List<UserListItemViewModel> Users { get; set; } = new();
}

public class UserViewModel
{
    public bool IsSuccess { get; set; }
    public UserListItemViewModel User { get; set; } = new();
}

public class UserListItemViewModel
{
    public long Id { get; set; }
    public string? Forename { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public DateOnly DateOfBirth {  get; set; }
    public bool IsActive { get; set; }
}
