using Axpo;
using Microsoft.Extensions.Logging;
using TradingSystem.Application.UseCases;

namespace TradingSystem.Infrastructure.Ports;

public sealed class TradingSystemApp
{
    private readonly ICreateInterDayReport createInterDayReport;
    private readonly TradingSystemAppSettings settings;
    private readonly ILogger<TradingSystemApp> logger;

    private bool isRunning;

    public TradingSystemApp(
        TradingSystemAppSettings settings,
        ICreateInterDayReport createInterDayReport,
        ILogger<TradingSystemApp> logger
    )
    {
        ArgumentNullException.ThrowIfNull(createInterDayReport, nameof(createInterDayReport));
        ArgumentNullException.ThrowIfNull(settings, nameof(settings));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        this.createInterDayReport = createInterDayReport;
        this.settings = settings;
        this.logger = logger;
        isRunning = false;
    }

    public async Task Start(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Trading System App started");
        isRunning = true;
        do
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                CreateInterDayReportRequest createInterDayReportRequest = new(DateTime.UtcNow.AddDays(1), this.settings.TimeZone);
                logger.LogInformation("Executing CreateInterDayReport use case");
                await createInterDayReport.Execute(createInterDayReportRequest, cancellationToken);
                logger.LogInformation("CreateInterDayReport use case executed successfully");
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("User requested to stop the Trading System App");
                isRunning = false;
            }
            catch (PowerServiceException ex)
            {
                logger.LogError(ex, "An error occurred while creating the inter-day report");
            }

            if (isRunning)
            {
                await Task.Delay(settings.SecondsBetweenReports, cancellationToken);
            }
        } while (this.isRunning);
        logger.LogInformation("Trading System App stopped");
    }

    public void Stop()
    {
        logger.LogInformation("Trading System App stopping...");
        isRunning = false;
    }
}
