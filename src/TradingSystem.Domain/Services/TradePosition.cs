namespace TradingSystem.Domain.Services;

public sealed class TradePosition
{
    public readonly int PeriodId;

    public readonly double Volume;

    public TradePosition(int periodId, double volume)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(periodId, 1, nameof(periodId));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(periodId, 24, nameof(periodId));
        PeriodId = periodId;
        Volume = volume;
    }
}
