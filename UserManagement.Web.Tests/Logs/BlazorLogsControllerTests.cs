using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Logs.Models;
using UserManagement.Services.Logs.Services;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Web.Tests.Logs;
public class BlazorLogsControllerTests
{
    private CancellationToken _defaultCancellationToken => new CancellationTokenSource().Token;
    private List<User> _users = CreateUsers();

    [Fact]
    public async Task List_DefaultPassedInShouldReturnAllLogs_InUserLisViewModel()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var logs = CreateLogs();
        var logService = CreateMockLogService(logs);
        BlazorLogsController controller = new(logService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(new(), _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var expectedLogs = logs.Select(l => (LogDTO)l).ToList();
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<LogListViewModel>()
            .Which.Logs.Should().BeEquivalentTo(expectedLogs, o => o.Excluding(l => l.DateofAction));
    }

    private static Mock<ILogService> CreateMockLogService(List<Log> logs)
    {
        Mock<ILogService> logService = new();
        logService.Setup(ls => ls.GetLogs(It.IsAny<LogListViewModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(logs);
        return logService;
    }


    private List<Log> CreateLogs() => new List<Log>
            {
                new Log { Id = 5, DateofAction =  DateTime.Now, Details = "New User Added {bob friend}", Type = 1, UserID = 5, SnapShot =  CreateSnapShot(5)  },
                new Log { Id = 6, DateofAction =  DateTime.Now, Details = "edited User {bob friend}", Type = 2, UserID = 5, SnapShot =  CreateSnapShot(5)  },
                new Log { Id = 5, DateofAction =  DateTime.Now, Details = "deleted User Added {bob friend}", Type = 1, UserID = 5, SnapShot =  CreateSnapShot(5)  },
                new Log { Id = 7, DateofAction =  DateTime.Now, Details = "new User {bob friend1}", Type = 1, UserID = 6, SnapShot =  CreateSnapShot(6) },
                new Log { Id = 8, DateofAction =  DateTime.Now, Details = "new User {bob friend2}", Type = 1, UserID = 7, SnapShot = CreateSnapShot(7)  },

            };

    private string CreateSnapShot(int userID)
    {
        User user = _users.Where(u => u.Id == userID).First();
        return JsonSerializer.Serialize(user);
    }

    private static List<User> CreateUsers() =>
            new List<User>()
            {
            new User
            {
                Id = 5,
                Forename = "test 1",
                Surname = "test 1",
                Email = "test@email.com",
                IsActive = false,
            },
              new User
            {
                  Id = 6,
                Forename = "test 2",
                Surname = "test 2",
                Email = "test2@email.com",
                IsActive = true,
            }
              ,  new User
            {
                  Id = 7,
                Forename = "test 1",
                Surname = "test 1",
                Email = "test2@email.com",
                IsActive = false,
            }
            };
}
