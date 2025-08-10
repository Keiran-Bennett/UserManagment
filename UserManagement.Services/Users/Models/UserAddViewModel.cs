namespace UserManagement.Services.Users.Models;

public class UserAddViewModel
{
    public bool IsSuccess { get; set; }
    public UserFormDTO User { get; set; } = new();
}
