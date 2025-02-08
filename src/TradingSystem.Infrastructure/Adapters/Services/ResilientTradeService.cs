using TradingSystem.Domain.Services;

namespace TradingSystem.Infrastructure.Adapters.Services;

public sealed class ResilientTradeService : ITradeService
{
    private readonly ITradeService innerService;

    private readonly ResilientTradeServiceSettings settings;

    public ResilientTradeService(ResilientTradeServiceSettings settings, ITradeService innerService)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(innerService);
        this.innerService = innerService;
        this.settings = settings;
    }

    public async Task<TradePositions> GetPositionsByDate(DateTime date, CancellationToken cancellationToken = default)
    {
        // We can use Polly to handle retries, circuit breakers, etc. but for this example we will use a simple retry mechanism
        TradePositions? tradePositions = null;
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                tradePositions = await this.innerService.GetPositionsByDate(date, cancellationToken);
            }
            catch
            {
                await Task.Delay(this.settings.SecondsBetweenRetries, cancellationToken);
            }
        } while (tradePositions is null);
        return tradePositions!;
    }
}
