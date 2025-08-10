using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Users.Models;
using UserManagement.Services.Users.Services;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Web.Tests.Users;


public class UserControllerTests
{
    private readonly Mock<IUserService> _userService = new();
    private CancellationToken _defaultCancellationToken => new CancellationTokenSource().Token;
    private UsersController CreateListUsersController()
    {
        var controller = new UsersController(_userService.Object);
        return controller;
    }


    [Fact]
    public async Task List_NoTypePassedInShouldReturnAllUsers_InUserLisViewModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateListUsersController();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(0, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetAll(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<ViewResult>()
        .Which.Model.Should().BeOfType<UserListViewModel>();
    }



    [Fact]
    public async Task List_ActiveUsersPassedInShouldReturnOneUser_InUserLisViewModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateListUsersController();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(2, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetActiveUsers(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<ViewResult>()
        .Which.Model.Should().BeOfType<UserListViewModel>();
    }

    [Fact]
    public async Task List_InActiveUsersPassedInShouldReturnOneUser_InUserLisViewModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateListUsersController();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(3, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetInActiveUsers(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<ViewResult>()
      .Which.Model.Should().BeOfType<UserListViewModel>();
    }

    [Fact]
    public async Task View_GetUser_MappedTheUserToViewModel_RazorPage_ViewModelShouldEqualUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var returnUser = new User { Forename = "test", Surname = "test" };
        _userService.Setup(us => us.GetUser(1, It.IsAny<CancellationToken>())).ReturnsAsync(returnUser);
        var controller = new UsersController(_userService.Object);


        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.View(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetUser(1, It.IsAny<CancellationToken>()), Times.Once);
        var model = result.Should().BeOfType<ViewResult>()
         .Which.Model.Should().BeOfType<UserViewModel>().Which;
        model.IsSuccess.Should().BeTrue();
        model.User.Should().BeEquivalentTo((UserDTO)returnUser);
    }

    [Fact]
    public async Task Delete_GetUser_UserIsNull_Razor_ReturnViewModel_IsSuccessIsFalse()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _userService.Setup(us => us.GetUser(1, It.IsAny<CancellationToken>())).ReturnsAsync((User)null!);
        var controller = new UsersController(_userService.Object);


        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Delete(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetUser(1, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<ViewResult>()
      .Which.Model.Should().BeOfType<UserDeleteViewModel>()
      .Which.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_GetUser_UserIsPopulated_Razor_ReturnViewModel_IsSuccessIsTrueAndPopulatedModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var returnUser = new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new System.DateOnly(2010, 2, 25) };
        _userService.Setup(us => us.GetUser(1, It.IsAny<CancellationToken>())).ReturnsAsync(returnUser);
        var controller = new UsersController(_userService.Object);


        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Delete(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetUser(1, It.IsAny<CancellationToken>()), Times.Once);
        var model = result.Should().BeOfType<ViewResult>()
       .Which.Model.Should().BeOfType<UserDeleteViewModel>().Which;
        model.IsSuccess.Should().BeTrue();
        model.User.Should().BeEquivalentTo((UserDTO)returnUser);
    }

    [Fact]
    public async Task ConfirmDelete_UserDelete_Razor_Success_ReturnmodelWithSuccessFlag()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _userService.Setup(us => us.DeleteUser(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new UsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmDelete(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.DeleteUser(1, It.IsAny<CancellationToken>()), Times.Once);
        var model = result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task ConfirmDelete_UserDelete_Razor_Failure_ReturnmodelWithSuccessFlag()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _userService.Setup(us => us.DeleteUser(1, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var controller = new UsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmDelete(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.DeleteUser(1, It.IsAny<CancellationToken>()), Times.Once);
        var model = result.Should().BeOfType<ViewResult>()
       .Which.Model.Should().BeOfType<UserDeleteViewModel>().Which;
        model.IsSuccess.Should().BeFalse();
    }


    [Fact]
    public void Add_Razor_ReturnViewModel_IsSuccessIsTrue()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = new UsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.Add();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<ViewResult>()
      .Which.Model.Should().BeOfType<UserAddViewModel>()
      .Which.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ConfirmAdd_Razor_ReturnViewModel_IsRedirectResukt()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IUserService> userService = new();
        userService.Setup(us => us.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new UsersController(userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmAdd(new(), _defaultCancellationToken);

        // Assert: Verifies that the action of
        // the method under test behaves as expected.
        result.Should().BeOfType<RedirectToActionResult>();
    }


    [Fact]
    public async Task ConfirmAdd_Razor_InvalidModel_IsRedirectResult()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = new UsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmAdd(CreateAddModel(), _defaultCancellationToken);

        result.Should().BeOfType<RedirectToActionResult>();
    }


    [Fact]
    public async Task ConfirmAdd_ServiceSucceeds_IsRedirectResult()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _userService.Setup(us => us.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new UsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmAdd(CreateAddModel(), _defaultCancellationToken);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task ConfirmAdd_ServiceFails_IsRedirectResult()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IUserService> userService = new();
        userService.Setup(us => us.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var controller = new UsersController(userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmAdd(CreateAddModel(), _defaultCancellationToken);

        result.Should().BeOfType<RedirectToActionResult>();
    }

    private static UserAddViewModel CreateAddModel() => new UserAddViewModel()
    {
        User = new UserFormDTO
        {
            Surname = "Test",
            Email = "test",
        }
    };
}


