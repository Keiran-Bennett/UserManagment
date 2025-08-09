namespace UserManagement.Web.Models.Users;

public class UserEditViewModel
{
    public bool IsSuccess { get; set; }
    public UserDTO User { get; set; } = new();
}
