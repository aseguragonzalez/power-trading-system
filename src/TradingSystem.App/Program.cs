using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradingSystem.Infrastructure.Ports;

namespace TradingSystem.App;

class Program
{
    static async Task Main(string[] args)
    {
        var timeZoneId = GetTimeZoneIdFromArgs(args);
        var secondsBetweenReports = GetSecondsBetweenReports(args);
        var retrySeconds = GetSecondsFromArgs(args);
        var path = GetPathFromArgs(args);

        ServiceProvider serviceProvider = ConfigureServices(
            new ServiceCollection(),
            timeZoneId: timeZoneId,
            secondsBetweenReports: secondsBetweenReports,
            retrySeconds: retrySeconds,
            path: path
        );

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation(
            "Args: timeZoneId={timeZoneId}, secondsBetweenReports={secondsBetweenReports}, retrySeconds={retrySeconds}, path={path}",
            timeZoneId, secondsBetweenReports, retrySeconds, path
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

    static string? GetTimeZoneIdFromArgs(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-t":
                case "--timezone":
                    if (i + 1 < args.Length)
                    {
                        return args[i + 1];
                    }
                    break;
            }
        }
        return null;
    }

    static int? GetSecondsBetweenReports(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-s":
                case "--seconds":
                    if (i + 1 < args.Length)
                    {
                        return int.Parse(args[i + 1]);
                    }
                    break;
            }
        }
        return null;
    }

    static string? GetPathFromArgs(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-p":
                case "--path":
                    if (i + 1 < args.Length)
                    {
                        return args[i + 1];
                    }
                    break;
            }
        }
        return null;
    }

    static int? GetSecondsFromArgs(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-r":
                case "--retries":
                    if (i + 1 < args.Length)
                    {
                        return int.Parse(args[i + 1]);
                    }
                    break;
            }
        }
        return null;
    }

    static ServiceProvider ConfigureServices(
        IServiceCollection services,
        string? timeZoneId = null,
        int? secondsBetweenReports = null,
        string? path = null,
        int? retrySeconds = null
    ) =>
        services
            .AddLogging(builder => builder.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
            .AddTradingSystemAppSettings(timeZoneId: timeZoneId, secondsBetweenReports: secondsBetweenReports)
            .AddCsvReportRepositorySettings(path: path)
            .AddResilientTradeServiceSettings(secondsBetweenRetries: retrySeconds)
            .AddUseCases()
            .AddAdapters()
            .AddPorts()
            .BuildServiceProvider();
}
