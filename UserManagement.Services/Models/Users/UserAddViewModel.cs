namespace UserManagement.Web.Models.Users;

public class UserAddViewModel
{
    public bool IsSuccess { get; set; }
    public UserFormDTO User { get; set; } = new();
}
