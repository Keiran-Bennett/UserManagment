using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Tests;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    private Mock<IDataContext> _dataContext => SetupActiveUsersDataModels();
    private readonly Mock<ILogService> _loggerService = new();
    private UserService CreateService() => new(_dataContext.Object, _loggerService.Object);
    private UserService CreateServiceWithMockedErrors()
    {
        Mock<IDataContext> dataContextWithErrors = new();

        dataContextWithErrors.Setup(s => s.GetAll<User>()).Throws(new System.Exception("test exception"));
        return new(dataContextWithErrors.Object, _loggerService.Object);
    }

    [Fact]
    public async Task GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var cts = new CancellationTokenSource();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAll(cts.Token);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(CreateUsers().ToList());
    }

    [Fact]
    public async Task GetAll_Error_ReturnList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateServiceWithMockedErrors();
        var cts = new CancellationTokenSource();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAll(cts.Token);
        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(new List<User> { });
    }

    [Fact]
    public async Task GetActiveUsers_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var cts = new CancellationTokenSource();
        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetActiveUsers(cts.Token);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(CreateUsers().Where(u => u.IsActive).ToList());
    }

    [Fact]
    public async Task GetActiveUsers_Error_ReturnEmptyList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateServiceWithMockedErrors();
        var cts = new CancellationTokenSource();
        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAll(cts.Token);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(new List<User> { });
    }

    [Fact]
    public async Task GetInActiveUsers_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var cts = new CancellationTokenSource();
        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetInActiveUsers(cts.Token);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(CreateUsers().Where(u => !u.IsActive).ToList());
    }


    [Fact]
    public async Task GetInActiveUsers_Error_ReturnEmptyList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateServiceWithMockedErrors();
        var cts = new CancellationTokenSource();
        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetInActiveUsers(cts.Token);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(new List<User> { });
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



    [Fact]
    public async Task GetUser_Error_RethrowsException()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _dataContext.Setup(s => s.Get<User>(It.IsAny<Expression<Func<User, bool>>>(),It.IsAny<CancellationToken>())).Throws(new System.Exception("test exception"));
        var service = CreateServiceWithMockedErrors();
        var cts = new CancellationTokenSource();
        // Act: Invokes the method under test with the arranged parameters.
        Func<Task<User>> result = async () => await service.GetUser(1,cts.Token);

        // Assert: Verifies that the action of the method under test behaves as expected.
        await result.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetUser_Success_ReturnsUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContext = new();
        dataContext
            .Setup(s => s.Get<User>(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = 2, Forename = "bob", Surname = "Jones" });
       var service = new UserService(dataContext.Object, _loggerService.Object);
        var cts = new CancellationTokenSource();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetUser(2, cts.Token);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(new User { Id = 2, Forename = "bob", Surname = "Jones" });
        _loggerService.Verify(l => l.GetAllLogsPerUser(2), Times.Once);
    }
}
