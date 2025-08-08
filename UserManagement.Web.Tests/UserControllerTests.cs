using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(1);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Users.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task View_WhenServiceReturnsUser_ReturnViewModelWithSuccess()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        User user = SetUpGetUser();
        UserListItemViewModel userViewModel = (UserListItemViewModel)user;
        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.View(5);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserViewModel>()
            .Which.User.Should().BeEquivalentTo(userViewModel)
            .Should().BeOfType<UserViewModel>()
            .Which.IsSuccess.Should().BeTrue();
        
    }

    [Fact]
    public async Task View_WhenServiceReturnsUser_ReturnViewModelWithFailure()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        User user = SetUpGetUser();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.View(4);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserViewModel>()
            .Which.IsSuccess.Should().BeFalse(); 
    }


    [Fact]
    public async Task Delete_WhenServiceReturnsUser_ReturnViewModelWithSuccess()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        User user = SetUpGetUser();
        UserListItemViewModel userViewModel = (UserListItemViewModel)user;
        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Delete(5);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserDeleteViewModel>()
            .Which.User.Should().BeEquivalentTo(userViewModel)
            .Should().BeOfType<UserDeleteViewModel>()
            .Which.IsSuccess.Should().BeTrue();

    }

    [Fact]
    public async Task Delete_WhenServiceReturnsUser_ReturnViewModelWithFailure()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        User user = SetUpGetUser();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Delete(4);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserDeleteViewModel>()
            .Which.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ConfirmDelete_WhenServiceReturnsUser_Redirect()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();

        _userService
            .Setup(u => u.DeleteUser(It.Is<long>(l => l == 4)))
            .Returns(Task.FromResult(true));

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmDelete(4);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = result as RedirectToActionResult;
        redirectResult!.ActionName.Should().Be("List");
    }

    [Fact]
    public async Task ConfirmDelete_WhenServiceReturnsNullUser_RedirectTopage()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller =  CreateController();

        _userService
            .Setup(u => u.DeleteUser(It.IsAny<long>()))
            .Returns(Task.FromResult(false));

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ConfirmDelete(4);

        // Assert: Verifies that the action of the method under test behaves as expected.

        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = result as ViewResult;
        redirectResult!.Model.Should()
            .BeOfType<UserDeleteViewModel>()
               .Which.IsSuccess.Should().BeFalse();
    }


    private User SetUpGetUser()
    {
        var user = new User
        {
            Id = 5,
            Forename = "test 123",
            Surname = "test",
            Email = "test@email.com",
            DateOfBirth = new DateOnly(2000, 1, 24),
            IsActive = true
        };

        _userService
            .Setup(s => s.GetUser(It.Is<long>(id => id == 5)))
            .Returns(Task.FromResult((User?) user));

        return user;
    }

    private List<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        IEnumerable<User> users = new List<User>
        {
            new User
            {
                Id = 5,
                Forename = forename,
                Surname = surname,
                Email = email,
                DateOfBirth = new DateOnly(2000,1,24),
                IsActive = isActive
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(Task.FromResult(users));

        return users.ToList();
    }

    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ILogService> _logService = new();
    private UsersController CreateController() => new(_userService.Object);
}
