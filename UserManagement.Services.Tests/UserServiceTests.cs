using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    [Fact]
    public async Task GetActiveUsers_WhenContextReturnUsers_MustBeOnlyActiveUsers()
    {
        var service = CreateService();
        var users = SetupActiveUsersDataModels();

        var activeUsers = await service.GetActiveUsers();

        activeUsers.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetActiveUsers_WhenContextReturnUsers_MustBeOnlyInActiveUsers()
    {
        var service = CreateService();
        var users = SetupActiveUsersDataModels();

        var activeUsers = await service.GetInActiveUsers();

        activeUsers.Count().Should().Be(2);
    }

    private IQueryable<User> SetupActiveUsersDataModels()
    {
        var users = new[]
        {
            new User
            {
                Id = 1,
                Forename = "test 1",
                Surname = "test 1",
                Email = "test@email.com",
                IsActive = false,
            },
              new User
            {
                  Id = 2,
                Forename = "test 2",
                Surname = "test 2",
                Email = "test2@email.com",
                IsActive = true,
            }
              ,  new User
            {
                  Id = 3,
                Forename = "test 1",
                Surname = "test 1",
                Email = "test2@email.com",
                IsActive = false,
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private readonly Mock<ILogService> _loggerService = new();
    private UserService CreateService() => new(_dataContext.Object, _loggerService.Object);
}
