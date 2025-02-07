using TradingSystem.Domain.Services;

namespace TradingSystem.Domain;

public sealed class Report
{
    private readonly double[] volumes = new double[24];

    public readonly DateTime CreatedAt;

    public readonly DateTime Date;

    public readonly int Offset;

    public Report(DateTime createdAt, DateTime date, int offset = 0)
    {
        if (createdAt.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentOutOfRangeException(nameof(createdAt), "CreatedAt must be in UTC.");
        }

        if (date.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Date must be in UTC.");
        }

        if (date < createdAt)
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Date cannot be in the past.");
        }

        this.Date = date;
        this.CreatedAt = createdAt;
        this.Offset = offset;
    }

    public Report(DateTime date, int offset) : this(date: date, createdAt: DateTime.UtcNow, offset: offset) { }

    public string ReportName => $"PowerPosition_{this.Date:yyyyMMdd}_{this.CreatedAt:yyyyMMddHHmm}.csv";

    public IEnumerable<ReportPosition> GetPositions()
    {
        DateTime baseLine = new(this.CreatedAt.Year, this.CreatedAt.Month, this.CreatedAt.Day, this.CreatedAt.Hour + this.Offset, 0, 0);

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
