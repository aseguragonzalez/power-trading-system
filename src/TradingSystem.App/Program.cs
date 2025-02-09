using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradingSystem.Infrastructure.Ports;

namespace TradingSystem.App;

class Program
{
    static async Task Main(string[] args)
    {
        AppArgs appArgs = new(args);
        ServiceProvider serviceProvider = ConfigureServices(new ServiceCollection(), appArgs);

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation(
            "Args: timeZoneId={timeZoneId}, secondsBetweenReports={secondsBetweenReports}, retrySeconds={retrySeconds}, path={path}",
            appArgs.TimeZoneId, appArgs.SecondsBetweenReports, appArgs.RetrySeconds, appArgs.Path
        );

        var app = serviceProvider.GetRequiredService<TradingSystemApp>();
        using var cancellationTokenSource = new CancellationTokenSource();
        bool isRunning = true;

        do
        {
            try
            {
                await app.Start(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Operation canceled");
                isRunning = false;
                cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred");
            }
        } while (isRunning);

        app.Stop();
    }

    static ServiceProvider ConfigureServices(IServiceCollection services, AppArgs appArgs) =>
        services
            .AddLogging(builder => builder.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
            .AddTradingSystemAppSettings(timeZoneId: appArgs.TimeZoneId, secondsBetweenReports: appArgs.SecondsBetweenReports)
            .AddCsvReportRepositorySettings(path: appArgs.Path)
            .AddResilientTradeServiceSettings(secondsBetweenRetries: appArgs.RetrySeconds)
            .AddUseCases()
            .AddAdapters()
            .AddPorts()
            .BuildServiceProvider();
}
