namespace TradingSystem.Infrastructure.Adapters.Services;

public sealed class ResilientTradeServiceSettings
{
    public readonly int DelayBetweenRetries;

    public ResilientTradeServiceSettings(int delayBetweenRetries)
    {
        if (delayBetweenRetries < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(delayBetweenRetries));
        }
        this.DelayBetweenRetries = delayBetweenRetries;
    }
}
