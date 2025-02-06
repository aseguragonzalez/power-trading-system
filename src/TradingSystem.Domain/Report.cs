using TradingSystem.Domain.Services;

namespace TradingSystem.Domain;

public sealed class Report
{
    private readonly double[] volumes = new double[24];

    public readonly DateTime Date;

    public readonly DateTime CreatedAt;

    public Report(DateTime date, DateTime createdAt)
    {
        if (date < createdAt)
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Date cannot be in the past.");
        }

        Date = date;
        CreatedAt = createdAt;
    }

    public Report(DateTime date) : this(date, createdAt: DateTime.UtcNow) { }

    public IEnumerable<ReportPosition> GetPositions(int timeOffset = 0)
    {
        DateTime baseLine = new(this.CreatedAt.Year, this.CreatedAt.Month, this.CreatedAt.Day, this.CreatedAt.Hour + timeOffset, 0, 0);

        return this.volumes.Select((volume, offset) => new ReportPosition(baseLine.AddHours(offset + 1), volume));
    }

    public void AddTradePositions(TradePositions tradePositions)
    {
        foreach (TradePosition tradePosition in tradePositions.Positions)
        {
            this.volumes[tradePosition.PeriodId - 1] += tradePosition.Volume;
        }
    }
}
