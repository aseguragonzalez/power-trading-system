namespace TradingSystem.Domain;

public sealed class ReportPosition
{
    public readonly DateTime Period;

    public readonly double Volume;

    public ReportPosition(DateTime slot, double volume)
    {
        Period = slot;
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
