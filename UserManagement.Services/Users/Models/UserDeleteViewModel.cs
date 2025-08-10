namespace UserManagement.Services.Users.Models;

public class UserDeleteViewModel
{
    public bool IsSuccess { get; set; }
    public UserDTO User { get; set; } = new();
}
