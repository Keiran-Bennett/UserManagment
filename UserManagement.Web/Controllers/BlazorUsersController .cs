using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;



[Route("blazor/users")]
public class BlazorUsersController : Controller
{
    private readonly IUserService _userService;
    public BlazorUsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("list")]
   public async Task<IActionResult> List(int userType, CancellationToken token)
    {
        var users = await GetUsers(userType,token);
        UserListViewModel model = MapUsersToViewModel(users);
        return Ok(model);
    }

    private async Task<IEnumerable<Models.User>> GetUsers(int userType, CancellationToken token)
    {
        UserRetrievalType userRetrievalType = (UserRetrievalType)userType;
        IEnumerable<Models.User> users = userRetrievalType switch
        {
            UserRetrievalType.Default => await _userService.GetAll(token),
            UserRetrievalType.ActiveUsers => await _userService.GetActiveUsers(token),
            UserRetrievalType.InActiveUsers => await _userService.GetInActiveUsers(token),
            _ => await _userService.GetAll(token)
        };
        return users;
    }

    private static UserListViewModel MapUsersToViewModel(IEnumerable<Models.User> users)
    {
        var items = users.Select(u => (UserDTO)u);
        return new UserListViewModel { Users = items.ToList() };
    }

    [HttpGet("View/{id:int}")]
    public async Task<IActionResult> View(int id, CancellationToken token)
    {
        var user = await _userService.GetUser(id,token);
        UserViewModel viewModel = CreateViewModelFromUser(user);
        return Ok(viewModel);
    }

    private static UserViewModel CreateViewModelFromUser(User? user)
    {
        if (user == null)
            return new UserViewModel { IsSuccess = false };
        else
            return new UserViewModel
            {
                User = (UserDTO)user,
                IsSuccess = true
            };
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        var user = await _userService.GetUser(id,token);
        UserDeleteViewModel viewModel = CreateDeleteViewModelFromUser(user);
        return Ok(viewModel);
    }

    private static UserDeleteViewModel CreateDeleteViewModelFromUser(User? user)
    {
        if (user == null)
            return new UserDeleteViewModel { IsSuccess = false };
        else
            return new UserDeleteViewModel
            {
                User = (UserDTO)user,
                IsSuccess = true
            };
    }

    [HttpPost("confirmdelete/{id:int}")]
    public async Task<IActionResult> ConfirmDelete(int id, CancellationToken cancellationToken)
    {
        bool IsSuccess = await _userService.DeleteUser(id,cancellationToken);
        if (!IsSuccess)
            return Ok(new UserDeleteViewModel { IsSuccess = false });
        else
            return Ok(new UserDeleteViewModel { IsSuccess = true });
    }


    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, CancellationToken token)
    {
        var user = await _userService.GetUser(id, token);
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
                User = (UserFormDTO)user,
                IsSuccess = true
            };
    }

    [HttpPost("confirmedit")]
    public async Task<IActionResult> ConfirmEdit(UserEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _userService.EditUser((User)model.User, cancellationToken);
        if (!result)
            return View(model);

        return RedirectToAction("View", new { id = model.User.Id });
    }

    [HttpPost("confirmadd")]
    public async Task<IActionResult> ConfirmAdd(UserFormDTO model, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
            return Ok(new UserAddViewModel { User = model, IsSuccess = true });

        var result = await _userService.AddUser((User) model, cancellationToken);
        return Ok(new UserAddViewModel { IsSuccess=result });
    }
}
