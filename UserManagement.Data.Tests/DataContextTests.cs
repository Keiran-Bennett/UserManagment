using System.Collections.Generic;
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
        entity.Should().BeEquivalentTo(expectedResult);
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
        entity.Should().BeEquivalentTo(CreateTestUsers());
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
            dc.Users.Should().BeEquivalentTo(expectedResult);
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

    private IDataContext CreateNamedDataContext(string name)
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase(databaseName: name)
        .Options;

        var dataContext = new DataContext(options);
        dataContext.Users.AddRange(CreateTestUsers());
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
}
