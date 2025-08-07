using System;
using System.Linq;
using UserManagement.Services;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List(int userType)
    {
        var users = GetUsers(userType);
        var model = MapUsersToViewModel(users);
        return View(model);
    }

    [HttpGet("{id:int}")]
    public ViewResult View(long id)
    {
        var user = _userService.GetUser(id);
        if(user is null)
        {
            return View(new UserViewModel { IsSuccess = false });
        }

        var viewModel = new UserViewModel
        {
            IsSuccess = true,
            User = new UserListItemViewModel
            {
                Id = id,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                Forename = user.Forename,
                Surname = user.Surname,
                IsActive = user.IsActive,
            }
        };
        return View(viewModel);

    }

    private static UserListViewModel MapUsersToViewModel(IEnumerable<Models.User> users)
    {
        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Users = items.ToList()
        };
        return model;
    }

    private IEnumerable<Models.User> GetUsers(int userType)
    {
        UserRetrievalType userRetrievalType = (UserRetrievalType)userType;
        IEnumerable<Models.User> users = userRetrievalType switch
        {
            UserRetrievalType.Default => _userService.GetAll(),
            UserRetrievalType.ActiveUsers => _userService.GetActiveUsers(),
            UserRetrievalType.InActiveUsers => _userService.GetInActiveUsers(),
            _ => throw new NotImplementedException()
        };
        return users;
    }
}
