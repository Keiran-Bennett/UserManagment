using System.Collections.Generic;

namespace UserManagement.Web.Models.Users;


public class UserListViewModel
{
    public List<UserDTO> Users { get; set; } = new();
}
