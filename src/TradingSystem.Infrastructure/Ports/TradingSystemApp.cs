using TradingSystem.Application;

namespace TradingSystem.Infrastructure.Ports;

public sealed class TradingSystemApp
{
    private readonly ICreateInterDayReport createInterDayReport;

    private readonly TradingSystemAppSettings settings;

    private bool isRunning;

    public TradingSystemApp(TradingSystemAppSettings settings, ICreateInterDayReport createInterDayReport)
    {
        ArgumentNullException.ThrowIfNull(createInterDayReport, nameof(createInterDayReport));
        ArgumentNullException.ThrowIfNull(settings, nameof(settings));
        this.createInterDayReport = createInterDayReport;
        this.settings = settings;
        this.isRunning = false;
    }

    public async Task Start(CancellationToken cancellationToken = default)
    {
        this.isRunning = true;
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                CreateInterDayReportRequest createInterDayReportRequest = new(DateTime.UtcNow, this.settings.TimeZone);
                await this.createInterDayReport.Execute(createInterDayReportRequest, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Log the cancellation
                this.isRunning = false;
            }
            catch (Exception)
            {
                // Log the exception
            }
            await Task.Delay(this.settings.SecondsBetweenReports, cancellationToken);

        } while (this.isRunning);
    }

    public void Stop()
    {
        this.isRunning = false;
    }
}
