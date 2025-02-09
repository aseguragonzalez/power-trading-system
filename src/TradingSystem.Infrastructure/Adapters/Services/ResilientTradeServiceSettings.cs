namespace TradingSystem.Infrastructure.Adapters.Services;

public sealed class ResilientTradeServiceSettings
{
    public TimeSpan SecondsBetweenRetries { get; }

    public ResilientTradeServiceSettings(int secondsBetweenRetries)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(secondsBetweenRetries, nameof(secondsBetweenRetries));
        SecondsBetweenRetries = TimeSpan.FromSeconds(secondsBetweenRetries);
    }
}
