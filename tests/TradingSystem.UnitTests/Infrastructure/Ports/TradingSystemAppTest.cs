using Axpo;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TradingSystem.Application.UseCases;
using TradingSystem.Infrastructure.Ports;

namespace TradingSystem.UnitTests.Infrastructure.Ports;

public sealed class TradingSystemAppTest
{
    [Fact]
    public void ShouldFailsWhenSettingsIsMissing()
    {
        // Arrange
        ILogger<TradingSystemApp> logger = Substitute.For<ILogger<TradingSystemApp>>();
        ICreateInterDayReport createInterDayReport = Substitute.For<ICreateInterDayReport>();
        Action act = () => _ = new TradingSystemApp(settings: null!, createInterDayReport, logger);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'settings')");
    }

    [Fact]
    public void ShouldFailsWhenLoggerIsMissing()
    {
        // Arrange
        ILogger<TradingSystemApp>? logger = null;
        TradingSystemAppSettings settings = new();
        ICreateInterDayReport createInterDayReport = Substitute.For<ICreateInterDayReport>();
        Action act = () => _ = new TradingSystemApp(settings, createInterDayReport, logger!);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'logger')");
    }

    [Fact]
    public void ShoudFailsWhenCreateInterDayReportIsMissing()
    {
        // Arrange
        ILogger<TradingSystemApp> logger = Substitute.For<ILogger<TradingSystemApp>>();
        TradingSystemAppSettings settings = new();
        Action act = () => _ = new TradingSystemApp(settings, createInterDayReport: null!, logger);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'createInterDayReport')");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Arrange
        ILogger<TradingSystemApp> logger = Substitute.For<ILogger<TradingSystemApp>>();
        TradingSystemAppSettings settings = new();
        ICreateInterDayReport createInterDayReport = Substitute.For<ICreateInterDayReport>();

        // Act
        TradingSystemApp tradingSystemApp = new(settings, createInterDayReport, logger);

        // Assert
        tradingSystemApp.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldCreateReportsAfterStart()
    {
        // Arrange
        ILogger<TradingSystemApp> logger = Substitute.For<ILogger<TradingSystemApp>>();
        TradingSystemAppSettings settings = new();
        ICreateInterDayReport createInterDayReport = Substitute.For<ICreateInterDayReport>();
        createInterDayReport.Execute(Arg.Any<CreateInterDayReportRequest>()).Returns(Task.CompletedTask);
        TradingSystemApp tradingSystemApp = new(settings, createInterDayReport, logger);

        // Act
        _ = Task.Run(async () => await tradingSystemApp.Start());

        // Assert
        await Task.Delay(2000);
        tradingSystemApp.Stop();
        await createInterDayReport.Received(1).Execute(Arg.Any<CreateInterDayReportRequest>());
    }

    [Fact]
    public async Task ShouldContinuesWhenSomeExceptionHappends()
    {
        // Arrange
        ILogger<TradingSystemApp> logger = Substitute.For<ILogger<TradingSystemApp>>();
        TradingSystemAppSettings settings = new(secondsBetweenReports: 1);
        ICreateInterDayReport createInterDayReport = Substitute.For<ICreateInterDayReport>();
        createInterDayReport
            .Execute(Arg.Any<CreateInterDayReportRequest>())
            .Returns(
                Task.FromException(new PowerServiceException("PowerServiceException")),
                [Task.CompletedTask, Task.CompletedTask, Task.CompletedTask]
            );
        TradingSystemApp tradingSystemApp = new(settings, createInterDayReport, logger);

        // Act
        _ = Task.Run(async () => await tradingSystemApp.Start());

        // Assert
        await Task.Delay(2000);
        tradingSystemApp.Stop();
        // Ensure Execute was called at least twice
        createInterDayReport.ReceivedCalls().Count().Should().BeGreaterThanOrEqualTo(2);
    }
}
