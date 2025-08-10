namespace UserManagement.Web.Models.Users;

public class UserEditViewModel
{
    public bool IsSuccess { get; set; }
    public UserFormDTO User { get; set; } = new();
}
