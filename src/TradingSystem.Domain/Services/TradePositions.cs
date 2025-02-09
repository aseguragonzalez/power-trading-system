namespace TradingSystem.Domain.Services;

public sealed class TradePositions
{
    public IEnumerable<TradePosition> Positions { get; }

    public TradePositions(IEnumerable<TradePosition>? positions)
    {
        Positions = positions ?? [];
    }

    public TradePositions() : this([])
    {
    }
}
