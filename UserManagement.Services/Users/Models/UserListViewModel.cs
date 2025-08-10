using System.Collections.Generic;

namespace UserManagement.Services.Users.Models;


public class UserListViewModel
{
    public List<UserDTO> Users { get; set; } = new();
}
