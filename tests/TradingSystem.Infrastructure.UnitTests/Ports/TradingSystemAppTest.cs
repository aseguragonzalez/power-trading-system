using FluentAssertions;
using NSubstitute;
using TradingSystem.Application;
using TradingSystem.Infrastructure.Ports;

namespace TradingSystem.Infrastructure.UnitTests.Ports;

public sealed class TradingSystemAppTest
{
    [Fact]
    public void ShouldFailsWhenSettingsIsMissing()
    {
        // Arrange
        ICreateInterDayReport createInterDayReport = Substitute.For<ICreateInterDayReport>();
        Action act = () => _ = new TradingSystemApp(settings: null!, createInterDayReport);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'settings')");
    }

    [Fact]
    public void ShoudFailsWhenCreateInterDayReportIsMissing()
    {
        // Arrange
        TradingSystemAppSettings settings = new();
        Action act = () => _ = new TradingSystemApp(settings, createInterDayReport: null!);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'createInterDayReport')");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        TradingSystemAppSettings settings = new();
        ICreateInterDayReport createInterDayReport = Substitute.For<ICreateInterDayReport>();

        // Act
        TradingSystemApp tradingSystemApp = new(settings, createInterDayReport);

        // Assert
        tradingSystemApp.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldCreateReportsAfterStart()
    {
        // Arrange
        TradingSystemAppSettings settings = new();
        ICreateInterDayReport createInterDayReport = Substitute.For<ICreateInterDayReport>();
        createInterDayReport.Execute(Arg.Any<CreateInterDayReportRequest>()).Returns(Task.CompletedTask);
        TradingSystemApp tradingSystemApp = new(settings, createInterDayReport);

        // Act
        _ = Task.Run(tradingSystemApp.Start);

        // Assert
        tradingSystemApp.Stop();
        await createInterDayReport.Received(1).Execute(Arg.Any<CreateInterDayReportRequest>());
    }

    [Fact]
    public async Task ShouldContinuesWhenSomeExceptionHappends()
    {
        // Arrange
        TradingSystemAppSettings settings = new(sscondsBewteenReports: 1);
        ICreateInterDayReport createInterDayReport = Substitute.For<ICreateInterDayReport>();
        createInterDayReport
            .Execute(Arg.Any<CreateInterDayReportRequest>())
            .ReturnsForAnyArgs(Task.FromException<Exception>(new Exception()), [Task.CompletedTask, Task.CompletedTask, Task.CompletedTask]);
        TradingSystemApp tradingSystemApp = new(settings, createInterDayReport);

        // Act
        _ = Task.Run(tradingSystemApp.Start);

        // Assert
        await Task.Delay(2000);
        tradingSystemApp.Stop();
        await createInterDayReport.ReceivedWithAnyArgs(2).Execute(Arg.Any<CreateInterDayReportRequest>());
    }
}
