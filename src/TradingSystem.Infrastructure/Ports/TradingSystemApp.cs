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

    public async Task Start()
    {
        this.isRunning = true;
        do
        {
            try
            {
                await createInterDayReport.Execute(
                    new CreateInterDayReportRequest(reportDate: DateTime.UtcNow, timeZoneId: this.settings.TimeZone.Id)
                );
            }
            catch (Exception)
            {
            }
            finally
            {
                await Task.Delay(this.settings.SecondsBetweenReports);
            }
        } while (this.isRunning);
    }

    public void Stop()
    {
        this.isRunning = false;
    }
}
