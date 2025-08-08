using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using UserManagement.Models;
using UserManagement.Services;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ViewResult> List(int userType)
    {
        var users = await GetUsers(userType);
        UserListViewModel model = MapUsersToViewModel(users);
        return View(model);
    }

    private async Task<IEnumerable<Models.User>> GetUsers(int userType)
    {
        UserRetrievalType userRetrievalType = (UserRetrievalType)userType;
        IEnumerable<Models.User> users = userRetrievalType switch
        {
            UserRetrievalType.Default => await _userService.GetAll(),
            UserRetrievalType.ActiveUsers => await _userService.GetActiveUsers(),
            UserRetrievalType.InActiveUsers => await _userService.GetInActiveUsers(),
            _ => await _userService.GetAll()
        };
        return users;
    }

    private static UserListViewModel MapUsersToViewModel(IEnumerable<Models.User> users)
    {
        var items = users.Select(u => (UserListItemViewModel)u);
        return new UserListViewModel { Users = items.ToList()};
    }

    [HttpGet("{id:int}")]
    public async Task<ViewResult> View(long id)
    {
        var user = await _userService.GetUser(id);
        UserViewModel viewModel = CreateViewModelFromUser(user);
        return View(viewModel);
    }

    private static UserViewModel CreateViewModelFromUser(User? user)
    {
        if (user == null)
            return new UserViewModel { IsSuccess = false };
        else
            return new UserViewModel
            {
                User = (UserListItemViewModel)user,
                IsSuccess = true
            };
    }

    [HttpGet("delete/{id:int}")]
    public async Task<ViewResult> Delete(long id)
    {
        var user = await _userService.GetUser(id);
        UserDeleteViewModel viewModel = CreateDeleteViewModelFromUser(user);
        return View(viewModel);
    }

    private static UserDeleteViewModel CreateDeleteViewModelFromUser(User? user)
    {
        if (user == null)
            return new UserDeleteViewModel { IsSuccess = false };
        else
            return new UserDeleteViewModel
            {
                User = (UserListItemViewModel)user,
                IsSuccess = true
            };
    }

    [HttpPost("confirmdelete")]
    public async Task<IActionResult> ConfirmDelete(long id)
    {
        bool IsSuccess = await _userService.DeleteUser(id);
        if (!IsSuccess)
            return View(new UserDeleteViewModel { IsSuccess = false });

        return RedirectToAction("List");
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetUser(id);
        var userViewModel = CreateEditViewModelFromUser(user);
        return View(userViewModel);
    }

    private static UserEditViewModel CreateEditViewModelFromUser(User? user)
    {
        if (user == null)
            return new UserEditViewModel { IsSuccess = false };
        else
            return new UserEditViewModel
            {
                User = (UserListItemViewModel)user,
                IsSuccess = true
            };
    }

    [HttpPost("confirmedit")]
    public async Task<IActionResult> ConfirmEdit(UserEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _userService.EditUser((User)model.User);
        if (!result)
            return View(model);

        return RedirectToAction("View", new { id = model.User.Id });
    }

    [HttpGet("/add")]
    public IActionResult Add()
    {
        return View(new UserAddViewModel() { IsSuccess = true });
    }

    [HttpPost("confirmadd")]
    public async Task<IActionResult> ConfirmAdd(UserEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        bool result = await _userService.AddUser((User)model.User);
        if (!result)
            return View(model);

        return RedirectToAction("List");
    }
}
