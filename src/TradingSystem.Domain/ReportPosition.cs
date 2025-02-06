namespace TradingSystem.Domain;

public sealed class ReportPosition
{
    public readonly DateTime Slot;

    public readonly double Volume;

    public ReportPosition(DateTime slot, double volume)
    {
        Slot = slot;
        Volume = volume;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Slot);
    }

    public override bool Equals(object? obj)
    {
        return obj is ReportPosition position && Slot == position.Slot;
    }
}
