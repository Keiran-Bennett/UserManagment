namespace UserManagement.Services.Users.Models;

public class UserViewModel
{
    public bool IsSuccess { get; set; }
    public UserDTO User { get; set; } = new();
}
