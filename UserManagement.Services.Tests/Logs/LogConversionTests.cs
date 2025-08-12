using System;
using System.Text.Json;
using UserManagement.Models;
using UserManagement.Services.Logs.Models;


namespace UserManagement.Web.Tests.Users;

public class LogConversionTests
{
    [Fact]
    public void Log_ConvertToViewModelToLog_ShouldHaveSameValues()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var log = CreateLog();

        // Act: Invokes the method under test with the arranged parameters.
        var logViewModel = (LogDTO)log;
        log = null;
        log = (Log)logViewModel;
        LogUserDTO? logUser = JsonSerializer.Deserialize<LogUserDTO>(log.SnapShot);

        // Assert: Verifies that the action of the method under test behaves as expected.
        logViewModel.Id.Should().Be(log.Id);
        logViewModel.UserID.Should().Be(log.UserID);
        logViewModel.DateofAction.Should().Be(log.DateofAction);
        logViewModel.Details.Should().Be(log.Details);
        logViewModel.LogType.Should().Be((LogType)log.Type);
        logViewModel.Snapshot.Should().BeEquivalentTo(logUser);       
    }

    private static LogUserDTO CreateLogUser()
    {
        return new LogUserDTO
        {
            Forename = "test 1",
            Surname = "test 2",
            Email = "testEmail@gmail.com",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
        };
    }
    [Fact]
    public void Log_ConvertUsersSnapShotJSON_Should_Be_Object()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var log = CreateLog();

        // Act: Invokes the method under test with the arranged parameters.
        var logViewModel = (LogDTO)log;

        // Assert: Verifies that the action of the method under test behaves as expected.
        logViewModel.Id.Should().Be(log.Id);
        logViewModel.UserID.Should().Be(log.UserID);
        logViewModel.DateofAction.Should().Be(log.DateofAction);
        logViewModel.Details.Should().Be(log.Details);
        logViewModel.LogType.Should().Be((LogType)log.Type);
        LogUserDTO? logUser = JsonSerializer.Deserialize<LogUserDTO>(log.SnapShot);
        logViewModel.Snapshot.Should().BeEquivalentTo(logUser);
    }

    private static Log CreateLog() => new Log
    {
        Id = 5,
        UserID = 6,
        DateofAction = DateTime.Now,
        Details = "Add to the user",
        Type = 2,
        SnapShot = JsonSerializer.Serialize(CreateLogUser()),
    };
}
