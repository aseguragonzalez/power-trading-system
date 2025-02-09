namespace TradingSystem.Domain.Entities;

public sealed class ReportPosition
{
    public DateTime Period { get; }

    public double Volume { get; }

    public ReportPosition(DateTime period, double volume)
    {
        Period = period;
        Volume = volume;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Period);
    }

    public override bool Equals(object? obj)
    {
        return obj is ReportPosition position && Period == position.Period;
    }
}
