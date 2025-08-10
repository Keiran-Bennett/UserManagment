using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Logs;
using UserManagement.Services.Tests;

namespace UserManagement.Services.Tests.Logs;

public class LogServiceTests
{
    private CancellationToken _defaultCancellationToken => new CancellationTokenSource().Token;

    [Fact]
    public async Task LogService_AddLog_Failure_ReturnFalse()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContextMock = new();
        dataContextMock.Setup(s => s.Create(It.IsAny<Log>(), It.IsAny<CancellationToken>())).Throws(new Exception("test"));
        LogService logService = new(dataContextMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await logService.AddLog(new AddLogRequest { UserID = 1, DateOfAction = DateTime.Today, Details = "New User Added", logType = LogType.Add }, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeFalse();
    }
    [Fact]
    public async Task LogService_GetUserLogs_Success_ReturnWith3InList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContextMock = new();
        dataContextMock.Setup(s => s.GetAll<Log>()).Returns(new TestAsyncEnumerable<Log>(CreateLogs().AsEnumerable()));
        LogService logService = new(dataContextMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await logService.GetAllLogsPerUser(5, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Count().Should().Be(3);
    }


    [Fact]
    public async Task LogService_GetUserLogs_Failure_ReturnEmptyList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContextMock = new();
        dataContextMock.Setup(s => s.GetAll<Log>()).Throws(new Exception("test"));
        LogService logService = new(dataContextMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await logService.GetAllLogsPerUser(1, _defaultCancellationToken);
        
        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Count().Should().Be(0);
    }

    [Fact]
    public async Task LogService_GetAllLogs_Success_ReturnListOfLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContextMock = new();
        dataContextMock.Setup(d => d.GetAll<Log>()).Returns(new TestAsyncEnumerable<Log>(CreateLogs().AsEnumerable()));
        LogService logService = new(dataContextMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await logService.GetLogs(new(), _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Count().Should().Be(5);
    }

    [Fact]
    public async Task LogService_GetAllLogs_Failure_ReturnEmptyList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContextMock = new();
        dataContextMock.Setup(d => d.GetAll<Log>()).Throws(new Exception("test"));
        LogService logService = new(dataContextMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await logService.GetLogs(new(), _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Count().Should().Be(0);
    }

    [Fact]
    public async Task LogService_GetAllLogs_Success_ReturnListOfLogsWithinDateTime()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContextMock = new();
        dataContextMock.Setup(d => d.GetAll<Log>()).Returns(new TestAsyncEnumerable<Log>(CreateLogs().AsEnumerable()));
        LogService logService = new(dataContextMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await logService.GetLogs(new() { StartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-7),  EndDate = DateOnly.FromDateTime(DateTime.Now) }, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task LogService_GetAllLogs_Success_ReturnListOfLogsWithinActionType()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContextMock = new();
        dataContextMock.Setup(d => d.GetAll<Log>()).Returns(new TestAsyncEnumerable<Log>(CreateLogs().AsEnumerable()));
        LogService logService = new(dataContextMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await logService.GetLogs(new() { Type = 2, IsFilterEnabled = true}, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Count().Should().Be(1);
    }

    [Fact]
    public async Task LogService_GetAllLogs_Success_ReturnListOfLogsWithSearchTerm()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        Mock<IDataContext> dataContextMock = new();
        dataContextMock.Setup(d => d.GetAll<Log>()).Returns(new TestAsyncEnumerable<Log>(CreateLogs().AsEnumerable()));
        LogService logService = new(dataContextMock.Object);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await logService.GetLogs(new() { SearchTerm = "2", IsFilterEnabled = true }, _defaultCancellationToken);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Count().Should().Be(1);
    }

    private static List<Log> CreateLogs() => new List<Log>
            {
                new Log { Id = 5, DateofAction =  DateTime.Now.AddDays(-2), Details = "New User Added {bob friend}", Type = 1, UserID = 5 },
                 new Log { Id = 6, DateofAction =  DateTime.Now.AddDays(-7), Details = "edited User {bob friend}", Type = 2, UserID = 5 },
                 new Log { Id = 9, DateofAction =  DateTime.Now.AddDays(-10), Details = "deleted User Added {bob friend}", Type = 1, UserID = 5 },
                 new Log { Id = 7, DateofAction =  DateTime.Now.AddDays(-10), Details = "new User {bob friend1}", Type = 1, UserID = 6 },
                   new Log { Id = 8, DateofAction =  DateTime.Now.AddDays(-10), Details = "new User {bob friend2}", Type = 1, UserID = 7 },

            };

}
