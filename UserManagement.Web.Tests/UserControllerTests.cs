using System;
using System.Security.Cryptography.X509Certificates;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List(1);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Users.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void View_WhenServiceReturnsUser_ReturnViewModelWithSuccess()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        User user = SetUpGetUser();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.View(5);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserViewModel>()
            .Which.User.Should().BeEquivalentTo(user)
            .Should().BeOfType<UserViewModel>()
            .Which.IsSuccess.Should().BeTrue();
        
    }

    [Fact]
    public void View_WhenServiceReturnsUser_ReturnViewModelWithFailure()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        User user = SetUpGetUser();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.View(4);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserViewModel>()
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
            .Returns(user);

        return user;
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
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
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
