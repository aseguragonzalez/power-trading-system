namespace TradingSystem.Domain.Services;

public sealed class TradePositions
{
    public readonly IEnumerable<TradePosition> Positions;

    public TradePositions(IEnumerable<TradePosition>? positions)
    {
        Positions = positions ?? [];
    }

    public TradePositions() : this([])
    {
    }
}
