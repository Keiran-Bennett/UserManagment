using System;
using System.ComponentModel.DataAnnotations;
using UserManagement.Models;

namespace UserManagement.Web.Models.Users;

public class UserEditViewModel
{
    public bool IsSuccess { get; set; }
    public UserFormDTO User { get; set; } = new();
}

public class UserFormDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Forename is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Forname must be at least 2 characters")]

    public string Forename { get; set; } = string.Empty;
    [Required(ErrorMessage = "Surname is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Surname must be at least 2 characters")]

    public string Surname { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Email must be at least 2 characters")]
    [EmailAddress(ErrorMessage = "Enter a valid Email")]
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public bool IsActive { get; set; }
    public static explicit operator UserFormDTO(User user) => new UserFormDTO
    {
        Id = user.Id,
        Forename = user.Forename,
        Surname = user.Surname,
        Email = user.Email,
        DateOfBirth = user.DateOfBirth,
        IsActive = user.IsActive,

    };

    public static explicit operator User(UserFormDTO model) => new User
    {
        Id = (int)model.Id,
        Forename = model.Forename,
        Surname = model.Surname,
        Email = model.Email,
        DateOfBirth = model.DateOfBirth,
        IsActive = model.IsActive,
    };
}
