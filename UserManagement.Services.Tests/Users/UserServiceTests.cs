using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Logs.Models;
using UserManagement.Services.Logs.Services;
using UserManagement.Services.Users.Services;

namespace UserManagement.Services.Tests.Users;


public class UserServiceTests
{
    private CancellationToken _defaultCancellationToken => new CancellationTokenSource().Token;
    private Mock<IDataContext> _dataContext => SetupActiveUsersDataModels();
    private readonly Mock<ILogService> _loggerService = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();

    [Fact]
    public async Task GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateStandardService();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAll(_defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(CreateUsers().ToList());
    }

    [Fact]
    public async Task GetAll_Error_ReturnEmptyList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateServiceWithMockedErrors();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAll(_defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(new List<User> { });
    }

    [Fact]
    public async Task GetActiveUsers_WhenContextReturnsEntities_MustReturnSameActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateStandardService();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetActiveUsers(_defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        List<User> activeUsers = CreateUsers().Where(u => u.IsActive).ToList();
        result.Should().BeEquivalentTo(activeUsers);
    }

    [Fact]
    public async Task GetActiveUsers_Error_ReturnEmptyList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateServiceWithMockedErrors();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAll(_defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(new List<User> { });
    }

    [Fact]
    public async Task GetInActiveUsers_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateStandardService();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetInActiveUsers(_defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        List<User> inActiveUsers = CreateUsers().Where(u => !u.IsActive).ToList();
        result.Should().BeEquivalentTo(inActiveUsers);
    }


    [Fact]
    public async Task GetInActiveUsers_Error_ReturnEmptyList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateServiceWithMockedErrors();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetInActiveUsers(_defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(new List<User> { });
    }

    [Fact]
    public async Task GetUser_Error_ThrrowsException()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateServiceWithMockedErrors();

        // Act: Invokes the method under test with the arranged parameters.
        Func<Task<User>> result = async () => await service.GetUser(1, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        await result.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetUser_Success_ReturnsUser()
    {
        var getUser = CreateGetUser();

        Mock<IDataContext> dataContext = new();
        _userRepoMock.Setup(u => u.GetUsersWithLogs(It.Is<int>(x => x == 2), It.IsAny<CancellationToken>())).ReturnsAsync(getUser);

        var service = new UserService(dataContext.Object, _loggerService.Object, _userRepoMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetUser(2, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(getUser);
    }

    private static User CreateGetUser() =>        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
            new User
            {
                Id = 2,
                Forename = "bob",
                Surname = "Jones",
                Logs = new List<Log> { new Log { UserID = 2, DateofAction = DateTime.Now } }
            };

    [Fact]
    public async Task AddUser_Success_ReturnTrueAndLogEntry()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var dataContext = MockDataContextWithIDMatching();
        var mockLogService = new Mock<ILogService>();
        mockLogService.Setup(l => l.AddLog(It.IsAny<CreateLogRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var service = new UserService(dataContext.Object, mockLogService.Object, _userRepoMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var addedUser = new User { Id = 5, Forename = "bob", Surname = "Jones" };
        var result = await service.AddUser(addedUser, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        string expectedJSON = JsonSerializer.Serialize((LogUserDTO)addedUser);
        mockLogService.Verify(l => l.AddLog(
                It.Is<CreateLogRequest>(r =>
                    r.UserID == 5 &&
                    r.Details.Contains($"bob Jones has been added") &&
                    r.JSONSnapShot == expectedJSON),
                It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AddUser_Failure_ThrowsException_ReturnFalse()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContext = new();
        dataContext
            .Setup(s => s.Create(It.IsAny<User>(), It.IsAny<CancellationToken>())).Throws(new Exception("Test catch"));
        var service = new UserService(dataContext.Object, _loggerService.Object, _userRepoMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.AddUser(new User { Id = 5, Forename = "bob", Surname = "Jones" }, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteUser_Failure_CantFindUser_ReturnFalse()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContext = new();
        dataContext
            .Setup(s => s.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);
        var service = new UserService(dataContext.Object, _loggerService.Object, _userRepoMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.DeleteUser(5, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteUser_Success_AddUser_CreateLog()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var mockLogService = new Mock<ILogService>();
        mockLogService.Setup(l => l.AddLog(It.IsAny<CreateLogRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var dataContext = MockDataContextWithIDMatching();
        var service = new UserService(dataContext.Object, mockLogService.Object, _userRepoMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.DeleteUser(5, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        dataContext.Verify(s => s.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()));
        mockLogService.Verify(l => l.AddLog(
            It.Is<CreateLogRequest>(r =>
                r.UserID == 5 &&
                r.Details == $"bob Jones has been deleted"),
            It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task EditUser_Failure_ThrowsException_ReturnFalse()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContext = new();
        dataContext
            .Setup(s => s.Update(It.IsAny<User>(), It.IsAny<CancellationToken>())).Throws(new Exception("Test catch"));

        // Act: Invokes the method under test with the arranged parameters.
        var service = new UserService(dataContext.Object, _loggerService.Object, _userRepoMock.Object);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var result = await service.EditUser(new User { Id = 5, Forename = "bob", Surname = "Jones" }, _defaultCancellationToken);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task EditUser_Success_AddDatabaseAndCreateLog()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var mockLogService = new Mock<ILogService>();
        mockLogService.Setup(l => l.AddLog(It.IsAny<CreateLogRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var dataContext = MockDataContextWithIDMatching();
        var service = new UserService(dataContext.Object, mockLogService.Object, _userRepoMock.Object);
        var editedUser = new User { Id = 5, Forename = "bob", Surname = "Jones" };

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.EditUser(editedUser, _defaultCancellationToken);


        // Assert: Verifies that the action of the method under test behaves as expected.
        string expectedJSON = JsonSerializer.Serialize((LogUserDTO)editedUser);
        mockLogService.Verify(l => l.AddLog(
        It.Is<CreateLogRequest>(r =>
            r.UserID == 5 &&
            r.Details.Contains($"bob Jones has been updated") &&
              r.JSONSnapShot == expectedJSON),
        It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeTrue();
    }

    private UserService CreateStandardService() => new(_dataContext.Object, _loggerService.Object, _userRepoMock.Object);
    private UserService CreateServiceWithMockedErrors()
    {
        Mock<IDataContext> dataContextWithErrors = new();
        dataContextWithErrors.Setup(s => s.GetAll<User>()).Throws(new Exception("test exception"));
        return new(dataContextWithErrors.Object, _loggerService.Object, _userRepoMock.Object);
    }

    private static Mock<IDataContext> MockDataContextWithIDMatching()
    {
        Mock<IDataContext> dataContext = new();
        dataContext
      .Setup(s => s.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync((Expression<Func<User, bool>> predicate, CancellationToken ct) =>
      {
          var func = predicate.Compile();
          var user = new User { Id = 5, Forename = "bob", Surname = "Jones" };

          if (func(user))
              return user;
          else
              return null;
      });
        return dataContext;
    }

    private Mock<IDataContext> SetupActiveUsersDataModels()
    {
        Mock<IDataContext> dataContext = new();
        var users = CreateUsers();
        dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(new TestAsyncEnumerable<User>(users));
        return dataContext;
    }

    private static IQueryable<User> CreateUsers() =>
            new[]
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

}
