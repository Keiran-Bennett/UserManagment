using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Users.Models;
using UserManagement.Services.Users.Services;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Web.Tests.Users;

public class BlazorUserControllerTests
{
    private readonly Mock<IUserService> _userService = new();
    private CancellationToken _defaultCancellationToken => new CancellationTokenSource().Token;

    [Fact]
    public async Task List_DefaultPassedInShouldReturnAllUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        BlazorUsersController controller = new(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetAll(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserListViewModel>();
    }

    [Fact]
    public async Task List_NoTypePassedInShould_CallAllUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = new BlazorUsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(0, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetAll(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserListViewModel>();
    }

    [Fact]
    public async Task List_ActiveUsersPassedInShouldCall_GetActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = new BlazorUsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(2, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetActiveUsers(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserListViewModel>();
    }

    [Fact]
    public async Task List_InActiveUsersPassedInShouldReturnOneUser_CallInactiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = new BlazorUsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(3, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetInActiveUsers(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>()
      .Which.Value.Should().BeOfType<UserListViewModel>();
    }


    [Fact]
    public async Task Delete_GetUser_UserIsNull_ReturnViewModel_IsSuccessIsFalse()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _userService.Setup(us => us.GetUser(1, It.IsAny<CancellationToken>())).ReturnsAsync((User)null!);
        var controller = new BlazorUsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Delete(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetUser(1, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>()
      .Which.Value.Should().BeOfType<UserDeleteViewModel>()
      .Which.IsSuccess.Should().BeFalse();
    }


    [Fact]
    public async Task ConfirmDelete_UserDelete_Failure_ReturnmodelWithSuccessFlag()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _userService.Setup(us => us.DeleteUser(1, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var controller = new BlazorUsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmDelete(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.DeleteUser(1, It.IsAny<CancellationToken>()), Times.Once);
        var model = result.Should().BeOfType<OkObjectResult>()
       .Which.Value.Should().BeOfType<UserDeleteViewModel>().Which;
        model.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_GetUser_UserIsPopulate_ReturnViewModel_IsSuccessIsFalse()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var returnUser = new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new System.DateOnly(2010, 2, 25) };
        _userService.Setup(us => us.GetUser(1, It.IsAny<CancellationToken>())).ReturnsAsync(returnUser);
        var controller = new BlazorUsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Delete(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetUser(1, It.IsAny<CancellationToken>()), Times.Once);
        var model = result.Should().BeOfType<OkObjectResult>()
       .Which.Value.Should().BeOfType<UserDeleteViewModel>().Which;
        model.IsSuccess.Should().BeTrue();
        model.User.Should().BeEquivalentTo((UserDTO)returnUser);
    }

    [Fact]
    public async Task View_GetUser_MappedTheUserToViewModel_BlazorPage_ViewModelShouldEqualUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var returnUser = new User { Forename = "test", Surname = "test" };
        _userService.Setup(us => us.GetUser(1, It.IsAny<CancellationToken>())).ReturnsAsync(returnUser);
        var controller = new BlazorUsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.View(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.GetUser(1, It.IsAny<CancellationToken>()), Times.Once);
        var model = result.Should().BeOfType<OkObjectResult>()
        .Which.Value.Should().BeOfType<UserViewModel>().Which;
        model.IsSuccess.Should().BeTrue();
        model.User.Should().BeEquivalentTo((UserDTO)returnUser);
    }

    [Fact]
    public async Task ConfirmDelete_UserDelete_Success_ReturnmodelWithSuccessFlag()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _userService.Setup(us => us.DeleteUser(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new BlazorUsersController(_userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmDelete(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(x => x.DeleteUser(1, It.IsAny<CancellationToken>()), Times.Once);
        var model = result.Should().BeOfType<OkObjectResult>()
       .Which.Value.Should().BeOfType<UserDeleteViewModel>().Which;
        model.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ConfirmAdd_ReturnViewModel_IsSuccessIsTrue()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateAddUserControllerWithSuccess();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmAdd(CreateUserFormModel(), _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<OkObjectResult>()
      .Which.Value.Should().BeOfType<UserAddViewModel>()
      .Which.IsSuccess.Should().BeTrue();
    }


    [Fact]
    public async Task ConfirmAdd_ServiceSucceeds_ReturnsSuccessOnModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateAddUserControllerWithSuccess();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmAdd(CreateUserFormModel(), _defaultCancellationToken);

        result.Should().BeOfType<OkObjectResult>()
           .Which.Value.Should().BeOfType<UserAddViewModel>()
     .Which.IsSuccess.Should().BeTrue();
    }

    private static BlazorUsersController CreateAddUserControllerWithSuccess()
    {
        Mock<IUserService> userService = new();
        userService.Setup(us => us.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new BlazorUsersController(userService.Object);
        return controller;
    }

    [Fact]
    public async Task ConfirmAdd_ServiceFails_ReturnsFalseOnModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateAddUserControllerWithFailure();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmAdd(CreateUserFormModel(), _defaultCancellationToken);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserAddViewModel>()
            .Which.IsSuccess.Should().BeFalse();
    }

    private static BlazorUsersController CreateAddUserControllerWithFailure()
    {
        Mock<IUserService> userService = new();
        userService.Setup(us => us.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var controller = new BlazorUsersController(userService.Object);
        return controller;
    }

    private static UserFormDTO CreateUserFormModel() =>
         new UserFormDTO
         {
             Surname = "Test",
             Email = "test",
         };


    [Fact]
    public async Task Edit_ServicePasses_ReturnsTrueOnModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.

        Mock<IUserService> userService = new();
        userService.Setup(us => us.GetUser(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new User { Id = 3, Forename = "test user", Logs = new List<Log> { } });
        var controller = new BlazorUsersController(userService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Edit(3, _defaultCancellationToken);

        var r = result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserEditViewModel>();

        r.Which.IsSuccess.Should().BeTrue();
        r.Which.User.Should().BeEquivalentTo((UserFormDTO)new User { Id = 3, Forename = "test user", Logs = new List<Log> { } });
    }

    [Fact]
    public async Task ConfirmEdit_ServiceFails_ReturnsFalseOnModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateEditUserControllerWithFailure();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmEdit(CreateUserFormModel(), _defaultCancellationToken);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserEditViewModel>()
            .Which.IsSuccess.Should().BeFalse();
    }

    private static BlazorUsersController CreateEditUserControllerWithFailure()
    {
        Mock<IUserService> userService = new();
        userService.Setup(us => us.EditUser(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var controller = new BlazorUsersController(userService.Object);
        return controller;
    }

    [Fact]
    public async Task ConfirmEdit_ServicePasses_ReturnsTrueOnModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateEditUserControllerWithSuccess();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmEdit(CreateUserFormModel(), _defaultCancellationToken);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<UserEditViewModel>()
            .Which.IsSuccess.Should().BeTrue();
     }

    private static BlazorUsersController CreateEditUserControllerWithSuccess()
    {
        Mock<IUserService> userService = new();
        userService.Setup(us => us.EditUser(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new BlazorUsersController(userService.Object);
        return controller;
    }
}


