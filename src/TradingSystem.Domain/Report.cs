using TradingSystem.Domain.Services;

namespace TradingSystem.Domain;

public sealed class Report
{
    private readonly double[] volumes = new double[24];

    public readonly DateTime CreatedAt;

    public readonly DateTime Date;

    public readonly TimeSpan Offset;

    public Report(DateTime createdAt, DateTime date, TimeSpan offset)
    {
        if (createdAt.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentOutOfRangeException(nameof(createdAt), "CreatedAt must be in UTC.");
        }

        if (date.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Date must be in UTC.");
        }

        ArgumentOutOfRangeException.ThrowIfLessThan(date, createdAt, nameof(date));

        Date = date;
        CreatedAt = createdAt;
        Offset = offset;
    }

    public Report(DateTime date, TimeSpan offset) : this(date: date, createdAt: DateTime.UtcNow, offset: offset) { }

    public Report(DateTime date, DateTime createdAt) : this(createdAt: createdAt, date: date, offset: TimeSpan.Zero) { }

    public string ReportName => $"PowerPosition_{this.Date:yyyyMMdd}_{this.CreatedAt:yyyyMMddHHmm}.csv";

    public IEnumerable<ReportPosition> GetPositions()
    {
        DateTime baseLine = new DateTime(CreatedAt.Year, CreatedAt.Month, CreatedAt.Day, CreatedAt.Hour, 0, 0).AddHours(this.Offset.Hours);

        return this.volumes.Select((volume, offset) => new ReportPosition(baseLine.AddHours(offset + 1), volume));
    }

    public void AddTradePositions(TradePositions tradePositions)
    {
        ArgumentNullException.ThrowIfNull(tradePositions);

        foreach (TradePosition tradePosition in tradePositions.Positions)
        {
            this.volumes[tradePosition.PeriodId - 1] += tradePosition.Volume;
        }
    }
}
