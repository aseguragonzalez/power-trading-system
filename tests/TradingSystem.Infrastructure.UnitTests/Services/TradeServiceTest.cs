using Axpo;
using FluentAssertions;
using NSubstitute;
using TradingSystem.Domain.Services;
using TradingSystem.Infrastructure.Services;

namespace TradingSystem.Infrastructure.UnitTests.Services;

public class TradeServiceTest
{
    [Fact]
    public void ShouldFailsWhenPowerServiceIsMissing()
    {
        // Arrange
        Action act = () => _ = new TradeService(null!);

        // Act and Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'powerService')");
    }

    [Fact]
    public void ShouldCreateAnInstance()
    {
        // Act
        TradeService service = new(powerService: Substitute.For<PowerService>());

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldRetrievePositionsCollectionByDate()
    {
        // Arrange
        DateTime date = DateTime.UtcNow;
        PowerTrade trades = PowerTrade.Create(date: date, numberOfPeriods: 24);
        IPowerService powerService = Substitute.For<IPowerService>();
        powerService.GetTradesAsync(date).ReturnsForAnyArgs(Task.FromResult<IEnumerable<PowerTrade>>([trades]));
        TradeService service = new(powerService);

        // Act
        TradePositions tradePositions = await service.GetPositionsByDate(DateTime.UtcNow);

        // Act & Assert
        tradePositions.Should().NotBeNull();
        tradePositions.Positions.Should().HaveCount(trades.Periods.Count());
    }

    [Fact]
    public async Task PowerServicePlayGround()
    {
        IPowerService powerService = new PowerService();
        try
        {
            DateTime dateTime = DateTime.UtcNow;
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var local = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);
            var powerTrades = await powerService.GetTradesAsync(local);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
