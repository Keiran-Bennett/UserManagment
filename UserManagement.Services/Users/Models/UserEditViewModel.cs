namespace UserManagement.Services.Users.Models;

public class UserEditViewModel
{
    public bool IsSuccess { get; set; }
    public UserFormDTO User { get; set; } = new();
}
