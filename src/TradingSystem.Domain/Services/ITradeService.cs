namespace TradingSystem.Domain.Services;

public interface ITradeService
{
    Task<TradePositions> GetPositionsByDate(DateTime date);
}
