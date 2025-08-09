namespace UserManagement.Web.Models.Users;

public class UserViewModel
{
    public bool IsSuccess { get; set; }
    public UserListItemViewModel User { get; set; } = new();
}
