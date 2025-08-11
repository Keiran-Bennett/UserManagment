using System;
using UserManagement.Models;
using UserManagement.Services.Users.Models;

namespace UserManagement.Services.Tests.Users;

public class UserConversionTests
{
    [Fact]
    public void User_ConvertToViewModelToUser_ShouldHaveSameValues()
    {
        var user = CreateUser();

        var userViewModel = (UserDTO) user;
        user = null;
        user = (User)userViewModel;


        userViewModel.Id.Should().Be(user.Id);
        userViewModel.Forename.Should().Be(user.Forename);
        userViewModel.Surname.Should().Be(user.Surname);
        userViewModel.Email.Should().Be(user.Email);
        userViewModel.DateOfBirth.Should().Be(user.DateOfBirth);
        userViewModel.IsActive.Should().Be(user.IsActive);
    }

    [Fact]
    public void User_FormDTO_ConvertToViewModelToUser_ShouldHaveSameValues()
    {
        var user = CreateUser();

        var userViewModel = (UserFormDTO)user;
        user = null;
        user = (User)userViewModel;


        userViewModel.Id.Should().Be(user.Id);
        userViewModel.Forename.Should().Be(user.Forename);
        userViewModel.Surname.Should().Be(user.Surname);
        userViewModel.Email.Should().Be(user.Email);
        userViewModel.DateOfBirth.Should().Be(user.DateOfBirth);
        userViewModel.IsActive.Should().Be(user.IsActive);
    }

    private static User CreateUser() =>
          new User
          {
              Id = 5,
              Forename = "Bob",
              Surname = "Jones",
              Email = "bob.jones@email.com",
              DateOfBirth = new DateOnly(2000, 1, 24),
              IsActive = false
          };
}
