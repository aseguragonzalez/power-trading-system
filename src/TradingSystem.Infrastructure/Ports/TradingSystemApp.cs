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
        this.isRunning = false;
    }

    public async Task Start(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Trading System App started");
        this.isRunning = true;
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                CreateInterDayReportRequest createInterDayReportRequest = new(DateTime.UtcNow.AddDays(1), this.settings.TimeZone);
                this.logger.LogInformation("Executing CreateInterDayReport use case");
                await this.createInterDayReport.Execute(createInterDayReportRequest, cancellationToken);
                this.logger.LogInformation("CreateInterDayReport use case executed successfully");
            }
            catch (OperationCanceledException)
            {
                this.logger.LogInformation("User requested to stop the Trading System App");
                // Log the cancellation
                this.isRunning = false;
            }
            catch (PowerServiceException ex)
            {
                this.logger.LogError(ex, "An error occurred while creating the inter-day report");
            }
            await Task.Delay(this.settings.SecondsBetweenReports, cancellationToken);

        } while (this.isRunning);
        this.logger.LogInformation("Trading System App stopped");
    }

    public void Stop()
    {
        this.isRunning = false;
    }
}
