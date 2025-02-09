using Axpo;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TradingSystem.Domain.Services;
using TradingSystem.Infrastructure.Adapters.Services;

namespace TradingSystem.UnitTests.Infrastructure.Adapters.Services;

public class ResilientTradeServiceTest
{
    [Fact]
    public void ShouldFailsWhenSettingsIsMissing()
    {
        // Arrange
        ILogger<ResilientTradeService> logger = Substitute.For<ILogger<ResilientTradeService>>();
        Action act = () => _ = new ResilientTradeService(settings: null!, Substitute.For<ITradeService>(), logger);

        // Act and Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'settings')");
    }

    [Fact]
    public void ShouldFailsWhenInnerServiceIsMissing()
    {
        // Arrange
        ILogger<ResilientTradeService> logger = Substitute.For<ILogger<ResilientTradeService>>();
        ResilientTradeServiceSettings settings = new(secondsBetweenRetries: 0);
        Action act = () => _ = new ResilientTradeService(settings, innerService: null!, logger);

        // Act and Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'innerService')");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Act
        ILogger<ResilientTradeService> logger = Substitute.For<ILogger<ResilientTradeService>>();
        ResilientTradeServiceSettings settings = new(secondsBetweenRetries: 0);
        ResilientTradeService service = new(settings, Substitute.For<ITradeService>(), logger);

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldRetrievePositionsCollectionByDate()
    {
        // Arrange
        DateTime date = DateTime.UtcNow;
        ResilientTradeServiceSettings settings = new(secondsBetweenRetries: 0);
        ILogger<ResilientTradeService> logger = Substitute.For<ILogger<ResilientTradeService>>();
        ITradeService innerService = Substitute.For<ITradeService>();
        innerService.GetPositionsByDate(date).ReturnsForAnyArgs(Task.FromResult(new TradePositions()));
        ResilientTradeService service = new(settings, innerService, logger);

        // Act
        TradePositions tradePositions = await service.GetPositionsByDate(DateTime.UtcNow);

        // Act & Assert
        tradePositions.Should().NotBeNull();
        tradePositions.Positions.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldRetryWhenInnerServiceFails()
    {
        // Arrange
        int retryCount = 3;
        var exceptionsResult = Enumerable.Range(0, retryCount).Select(
            _ => Task.FromException<TradePositions>(new PowerServiceException("PowerServiceException"))
        );
        DateTime date = DateTime.UtcNow;
        ResilientTradeServiceSettings settings = new(secondsBetweenRetries: 1);
        ILogger<ResilientTradeService> logger = Substitute.For<ILogger<ResilientTradeService>>();
        ITradeService innerService = Substitute.For<ITradeService>();
        innerService.GetPositionsByDate(date).ReturnsForAnyArgs<Task<TradePositions>>(
            Task.FromException<TradePositions>(new PowerServiceException("PowerServiceException")),
            [.. exceptionsResult, Task.FromResult(new TradePositions())]
        );
        ResilientTradeService service = new(settings, innerService, logger);

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        TradePositions tradePositions = await service.GetPositionsByDate(DateTime.UtcNow);
        stopwatch.Stop();

        // Act & Assert
        tradePositions.Should().NotBeNull();
        tradePositions.Positions.Should().BeEmpty();
        long expectedMilliseconds = settings.SecondsBetweenRetries.Seconds * (retryCount + 1) * 1000;
        stopwatch.ElapsedMilliseconds.Should().BeCloseTo(expectedMilliseconds, delta: 100);
    }
}
