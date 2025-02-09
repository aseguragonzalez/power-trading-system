using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradingSystem.Infrastructure.Ports;

namespace TradingSystem.App;

internal sealed class Program
{
    static async Task Main(string[] args)
    {
        AppArgs appArgs = new(args);
        ServiceProvider serviceProvider = ConfigureServices(new ServiceCollection(), appArgs);

        ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation(
            "Args: TimeZoneId={TimeZoneId}, Seconds={SecondsBetweenReports}, Retry={RetrySeconds}, Path={Path}",
            appArgs.TimeZoneId, appArgs.SecondsBetweenReports, appArgs.RetrySeconds, appArgs.Path
        );

        TradingSystemApp app = serviceProvider.GetRequiredService<TradingSystemApp>();
        using CancellationTokenSource cancellationTokenSource = new();
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("CTRL+C detected. Cancelling...");
            cancellationTokenSource.Cancel();
            e.Cancel = true;
        };

        try
        {
            await app.Start(cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Operation was canceled");
        }
        finally
        {
            app.Stop();
        }
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
