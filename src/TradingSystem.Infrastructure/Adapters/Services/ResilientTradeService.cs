using Axpo;
using Microsoft.Extensions.Logging;
using TradingSystem.Domain.Services;

namespace TradingSystem.Infrastructure.Adapters.Services;

public sealed class ResilientTradeService : ITradeService
{
    private readonly ITradeService innerService;
    private readonly ResilientTradeServiceSettings settings;
    private ILogger<ResilientTradeService> logger;

    public ResilientTradeService(
        ResilientTradeServiceSettings settings,
        ITradeService innerService,
        ILogger<ResilientTradeService> logger
    )
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(innerService);
        this.innerService = innerService;
        this.settings = settings;
        this.logger = logger;
    }

    public async Task<TradePositions> GetPositionsByDate(DateTime reportDate, CancellationToken cancellationToken = default)
    {
        // We can use Polly to handle retries, circuit breakers, etc. but for this example we will use a simple retry mechanism
        TradePositions? tradePositions = null;
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                this.logger.LogInformation("Getting positions by date: {ReportDate}", reportDate);
                tradePositions = await this.innerService.GetPositionsByDate(reportDate, cancellationToken);
                this.logger.LogInformation("Positions by date {ReportDate} retrieved successfully.", reportDate);
            }
            catch (PowerServiceException ex)
            {
                this.logger.LogError(ex, "An error occurred while trying to get positions by date");
                this.logger.LogWarning("We will wait {Seconds} seconds before retrying.", this.settings.SecondsBetweenRetries);
                await Task.Delay(this.settings.SecondsBetweenRetries, cancellationToken);
                this.logger.LogWarning("We will retry now.");
            }
        } while (tradePositions is null);
        return tradePositions!;
    }
}
