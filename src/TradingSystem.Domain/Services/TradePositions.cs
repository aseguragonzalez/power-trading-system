namespace TradingSystem.Domain.Services;

public sealed record TradePositions
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
