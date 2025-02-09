using Axpo;
using TradingSystem.Domain.Services;

namespace TradingSystem.Infrastructure.Adapters.Services;

public sealed class TradeService : ITradeService
{
    private readonly IPowerService powerService;

    public TradeService(IPowerService powerService)
    {
        ArgumentNullException.ThrowIfNull(powerService);
        this.powerService = powerService;
    }

    public async Task<TradePositions> GetPositionsByDate(DateTime reportDate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PowerTrade> powerTrades = await this.powerService.GetTradesAsync(reportDate);
        TradePositions tradePositions = new(
            positions: powerTrades.SelectMany(powerTrade =>
                powerTrade.Periods.Select(
                    period => new TradePosition(periodId: period.Period, volume: period.Volume)
                )
            )
        );
        return tradePositions;
    }
}
