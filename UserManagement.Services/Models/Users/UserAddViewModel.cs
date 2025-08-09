namespace UserManagement.Web.Models.Users;

public class UserAddViewModel
{
    public bool IsSuccess { get; set; }
    public UserDTO User { get; set; } = new();
}
