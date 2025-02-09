namespace TradingSystem.Domain.Services;

public sealed class TradePosition
{
    private const int FirstPeriodId = 1;
    private const int LastPeriodId = 24;

    public int PeriodId { get; }

    public double Volume { get; }

    public TradePosition(int periodId, double volume)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(periodId, FirstPeriodId, nameof(periodId));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(periodId, LastPeriodId, nameof(periodId));
        PeriodId = periodId;
        Volume = volume;
    }
}
