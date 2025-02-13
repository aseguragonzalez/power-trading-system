namespace TradingSystem.Domain.Services;

public interface ITradeService
{
    Task<TradePositions> GetPositionsByDate(DateTime reportDate, CancellationToken cancellationToken = default);
}
