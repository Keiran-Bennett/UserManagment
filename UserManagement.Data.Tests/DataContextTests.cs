using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    private CancellationToken _defaultCancellationToken => new CancellationTokenSource().Token;

    [Fact]
    public async Task GetEntity_WhenMatchesPredicate_UserShouldMatchOneInDB()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var dataContext = CreateNamedDataContext("1");

        // Act: Invokes the method under test with the arranged parameters.
        User? entity = await dataContext.Get<User>(u => u.Forename == "Robin", _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var expectedResult = new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", DateOfBirth = new System.DateOnly(2000, 2, 25), IsActive = true };
        entity.Should().BeEquivalentTo(expectedResult, op => op.Excluding(u => u.Logs));
        entity.Should().NotBeNull();
    }

    [Fact]
    public async Task GetEntity_WhenDoesNotMatchPredicate_UserShouldBeNull()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var dataContext = CreateNamedDataContext("2");

        // Act: Invokes the method under test with the arranged parameters.
        User? entity = await dataContext.Get<User>(u => u.Forename == "Robin2134", _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        entity.Should().BeNull();
    }

    [Fact]
    public async Task GetAllEntity_ShouldReturnAllUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var dataContext = CreateNamedDataContext("3");

        // Act: Invokes the method under test with the arranged parameters.
        List<User> entity = await dataContext.GetAll<User>().ToListAsync(_defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        entity.Should().BeEquivalentTo(CreateTestUsers(), op => op.Excluding(u => u.Logs));
    }

    [Fact]
    public async Task DeleteEntity_EntityShouldNotBeInList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var dataContext = CreateNamedDataContext("4");

        // Act: Invokes the method under test with the arranged parameters.
        User? deletedUser = await dataContext.Get<User>(u => u.Id == 1, _defaultCancellationToken);
        await dataContext.Delete(deletedUser!, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        if (dataContext is DataContext dc)
        {
            List<User> expectedResult = CreateTestUsers();
            expectedResult.RemoveAt(0);
            dc.Users.Should().BeEquivalentTo(expectedResult, op => op.Excluding(u => u.Logs));
        }
    }

    [Fact]
    public async Task UpdateEntity_EntityHasUpdatedValues()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var dataContext = CreateNamedDataContext("5");

        // Act: Invokes the method under test with the arranged parameters.
        User user = await dataContext.Get<User>(u => u.Id == 1, _defaultCancellationToken) ?? new User();
        user.Email = "testupdate";
        await dataContext.Update(user, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        User? retrievedUser = await dataContext.Get<User>(u => u.Id == 1, _defaultCancellationToken) ?? new User();
        retrievedUser.Should().NotBeNull();
        retrievedUser.Should().BeEquivalentTo(user);

    }

    [Fact]
    public async Task UserRepo_GetUserWithLogs_ReturnUsersWithLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test
        DataContext dc = CreateNamedDataContext("6") as DataContext ?? new();
        var userRepo = new UserRepository(dc);

        // Act: Invokes the method under test with the arranged parameters.
        var userWithLogs = await userRepo.GetUsersWithLogs(3,_defaultCancellationToken);

        userWithLogs.Should().NotBeNull();
        userWithLogs.Logs.Count().Should().Be(2);

    }

    private IDataContext CreateNamedDataContext(string name)
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase(databaseName: name)
        .Options;

        var dataContext = new DataContext(options);
        dataContext.Users.AddRange(CreateTestUsers());
        dataContext.Logs.AddRange(GetTestLogsWithUesrSnapshot());
        dataContext.SaveChanges();

        return dataContext;
    }

    private static List<User> CreateTestUsers() => new List<User> {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new System.DateOnly(2010, 2, 25) },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = new System.DateOnly(2008, 4, 25) },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth = new System.DateOnly(2005, 2, 25) },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth = new System.DateOnly(2002, 3, 25) },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth = new System.DateOnly(1980, 1, 2) },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth = new System.DateOnly(1992, 3, 25) },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false, DateOfBirth = new System.DateOnly(1990, 8, 25) },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false, DateOfBirth = new System.DateOnly(1996, 6, 26) },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false, DateOfBirth = new System.DateOnly(2024, 3, 18) },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true, DateOfBirth = new System.DateOnly(2001, 2, 25) },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true, DateOfBirth = new System.DateOnly(2000, 2, 25) },
        };

    public static List<Log> GetTestLogsWithUesrSnapshot()
    {

        var logs = CreateLogsRecords();
        AssignSnapshotsBasedOnUserID(logs);
        return logs;
    }

    private static void AssignSnapshotsBasedOnUserID(List<Log> logs)
    {
        var Users = CreateTestUsers();
        foreach (var log in logs)
            log.SnapShot = JsonSerializer.Serialize(Users.First(u => u.Id == log.UserID));
    }

    private static List<Log> CreateLogsRecords() => new List<Log>
    {
        new Log { Id = 1,  UserID = 1,  DateofAction = DateTime.Now.AddDays(-1),  Details = "Added User Peter Loew", Type = 1 },
        new Log { Id = 2,  UserID = 2,  DateofAction = DateTime.Now.AddDays(-2),  Details = "Added User Benjamin Franklin Gates", Type = 1 },
        new Log { Id = 3,  UserID = 3,  DateofAction = DateTime.Now.AddDays(-3),  Details = "Added User Castor Troy", Type = 1 },
        new Log { Id = 4,  UserID = 4,  DateofAction = DateTime.Now.AddDays(-4),  Details = "Added User Memphis Raines", Type = 1 },
        new Log { Id = 5,  UserID = 5,  DateofAction = DateTime.Now.AddDays(-5),  Details = "Added User Stanley Goodspeed", Type = 1 },
        new Log { Id = 6,  UserID = 6,  DateofAction = DateTime.Now.AddDays(-6),  Details = "Added User H.I. McDunnough", Type = 1 },
        new Log { Id = 7,  UserID = 7,  DateofAction = DateTime.Now.AddDays(-7),  Details = "Added User Cameron Poe", Type = 1 },
        new Log { Id = 8,  UserID = 8,  DateofAction = DateTime.Now.AddDays(-8),  Details = "Added User Edward Malus", Type = 1 },
        new Log { Id = 9,  UserID = 9,  DateofAction = DateTime.Now.AddDays(-9),  Details = "Added User Damon Macready", Type = 1 },
        new Log { Id = 10, UserID = 10, DateofAction = DateTime.Now.AddDays(-10), Details = "Added User Johnny Blaze", Type = 1 },
        new Log { Id = 11, UserID = 11, DateofAction = DateTime.Now.AddDays(-11), Details = "Added User Robin Feld", Type = 1 },

        new Log { Id = 13, UserID = 3,  DateofAction = DateTime.Now.AddDays(-13), Details = "Edited User Castor Troy", Type = 2 },
        new Log { Id = 14, UserID = 7,  DateofAction = DateTime.Now.AddDays(-14), Details = "Edited User Cameron Poe", Type = 2 },
        new Log { Id = 15, UserID = 1,  DateofAction = DateTime.Now.AddDays(-15), Details = "Edited User Peter Loew", Type = 2 },

        new Log { Id = 17, UserID = 2,  DateofAction = DateTime.Now.AddDays(-17), Details = "Deleted User Benjamin Franklin Gates", Type = 3 },
        new Log { Id = 18, UserID = 5,  DateofAction = DateTime.Now.AddDays(-18), Details = "Deleted User Stanley Goodspeed", Type = 3 },
        new Log { Id = 19, UserID = 8,  DateofAction = DateTime.Now.AddDays(-19), Details = "Deleted User Edward Malus", Type = 3 },

    };
}
