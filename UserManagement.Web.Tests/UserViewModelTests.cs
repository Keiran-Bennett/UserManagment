using System;
using UserManagement.Models;
using UserManagement.Web.Models.Users;

namespace UserManagement.Data.Tests;

public class UserViewModelTests
{
    [Fact]
    public void User_ConvertToViewModel_ShouldHaveSameValues()
    {
        User user = CreateUser();

        UserListItemViewModel userViewModel = (UserListItemViewModel)user;

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
