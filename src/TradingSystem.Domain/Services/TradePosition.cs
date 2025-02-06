namespace TradingSystem.Domain.Services;

public sealed record TradePosition
{
    public readonly int PeriodId;

    public readonly double Volume;

    public TradePosition(int periodId, double volume)
    {
        if (periodId < 1 || periodId > 24)
        {
            throw new ArgumentOutOfRangeException(nameof(periodId), "PeriodId must be between 1 and 24");
        }

        PeriodId = periodId;
        Volume = volume;
    }
}
