using Axpo;
using FluentAssertions;
using NSubstitute;
using TradingSystem.Domain.Services;
using TradingSystem.Infrastructure.Adapters.Services;

namespace TradingSystem.Infrastructure.UnitTests.Adapters.Services;

public class ResilientTradeServiceTest
{
    [Fact]
    public void ShouldFailsWhenSettingsIsMissing()
    {
        // Arrange
        Action act = () => _ = new ResilientTradeService(settings: null!, Substitute.For<ITradeService>());

        // Act and Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'settings')");
    }

    [Fact]
    public void ShouldFailsWhenInnerServiceIsMissing()
    {
        // Arrange
        ResilientTradeServiceSettings settings = new(delayBetweenRetries: 0);
        Action act = () => _ = new ResilientTradeService(settings, innerService: null!);

        // Act and Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'innerService')");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Act
        ResilientTradeServiceSettings settings = new(delayBetweenRetries: 0);
        ResilientTradeService service = new(settings, Substitute.For<ITradeService>());

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldRetrievePositionsCollectionByDate()
    {
        // Arrange
        DateTime date = DateTime.UtcNow;
        ResilientTradeServiceSettings settings = new(delayBetweenRetries: 0);
        ITradeService innerService = Substitute.For<ITradeService>();
        innerService.GetPositionsByDate(date).ReturnsForAnyArgs(Task.FromResult(new TradePositions()));
        ResilientTradeService service = new(settings, innerService);

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
        int retryCount = new Random().Next(1, 3);
        var exceptionsResult = Enumerable.Range(0, retryCount).Select(_ => Task.FromException<TradePositions>(new Exception()));
        DateTime date = DateTime.UtcNow;
        ResilientTradeServiceSettings settings = new(delayBetweenRetries: 1);
        ITradeService innerService = Substitute.For<ITradeService>();
        innerService.GetPositionsByDate(date).ReturnsForAnyArgs(
            Task.FromException<TradePositions>(new Exception()),
            [.. exceptionsResult, Task.FromResult(new TradePositions())]
        );
        ResilientTradeService service = new(settings, innerService);

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        TradePositions tradePositions = await service.GetPositionsByDate(DateTime.UtcNow);
        stopwatch.Stop();

        // Act & Assert
        tradePositions.Should().NotBeNull();
        tradePositions.Positions.Should().BeEmpty();
        stopwatch.ElapsedMilliseconds.Should().BeCloseTo(settings.DelayBetweenRetries * (retryCount + 1), delta: 10);
    }
}
