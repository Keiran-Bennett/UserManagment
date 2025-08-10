using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Web.Tests;
public class LogsControllerTests
{
    private CancellationToken _defaultCancellationToken => new CancellationTokenSource().Token;

    [Fact]
    public async Task List_DefaultPassedInShouldReturnAllLogs_InUserLisViewModel()
    {
        List<Log> logs = CreateLogs(); 

        Mock<ILogService> logService = new();
        logService.Setup(ls => ls.GetAllLogs(It.IsAny<CancellationToken>())).ReturnsAsync
            (logs);
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        LogsController controller = new(logService.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(new(), _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.

        List<LogDTO> expectedLogs = logs.Select(l => (LogDTO)l).ToList();
        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<LogListViewModel>()
            .Which.Logs.Should().BeEquivalentTo(expectedLogs, o => o.Excluding(l => l.DateofAction));
    }

    private static List<Log> CreateLogs() => new List<Log>
            {
                new Log { Id = 5, DateofAction =  DateTime.Now, Details = "New User Added {bob friend}", Type = 1, UserID = 5 },
                 new Log { Id = 6, DateofAction =  DateTime.Now, Details = "edited User {bob friend}", Type = 2, UserID = 5 },
                                 new Log { Id = 5, DateofAction =  DateTime.Now, Details = "deleted User Added {bob friend}", Type = 1, UserID = 5 },
                 new Log { Id = 7, DateofAction =  DateTime.Now, Details = "new User {bob friend}", Type = 1, UserID = 6 },
                   new Log { Id = 8, DateofAction =  DateTime.Now, Details = "new User {bob friend}", Type = 1, UserID = 7 },

            };
}
