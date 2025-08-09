namespace UserManagement.Web.Models.Users;

public class UserDeleteViewModel
{
    public bool IsSuccess { get; set; }
    public UserDTO User { get; set; } = new();
}
