using Axpo;
using TradingSystem.Domain.Repositories;
using TradingSystem.Domain.Services;
using TradingSystem.Infrastructure.Adapters.Repositories;
using TradingSystem.Infrastructure.Adapters.Services;
using TradingSystem.Infrastructure.Ports;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAdapters(this IServiceCollection services) =>
        services
            .AddSingleton<IReportRepository, CsvReportRepository>()
            .AddSingleton<IPowerService, PowerService>()
            .AddSingleton<TradeService, TradeService>()
            .AddSingleton<ITradeService>(sp => new ResilientTradeService(
                sp.GetRequiredService<ResilientTradeServiceSettings>(), sp.GetRequiredService<TradeService>()
            ));

    public static IServiceCollection AddCsvReportRepositorySettings(this IServiceCollection services, string? path) =>
        services.AddSingleton(
            _ => new CsvReportRepositorySettings(path ?? Environment.GetEnvironmentVariable("CSV_REPORTS_DIRECTORY")!)
        );

    public static IServiceCollection AddResilientTradeServiceSettings(this IServiceCollection services, int? secondsBetweenRetries) =>
        services.AddSingleton(_ =>
            new ResilientTradeServiceSettings(
                secondsBetweenRetries ?? int.Parse(Environment.GetEnvironmentVariable("SECONDS_BETWEEN_RETRIES")!)
            )
        );


    public static IServiceCollection AddPorts(this IServiceCollection services) =>
        services.AddSingleton<TradingSystemApp>();

    public static IServiceCollection AddTradingSystemAppSettings(this IServiceCollection services, int? secondsBetweenReports, string? timeZoneId) =>
        services.AddSingleton(_ =>
            new TradingSystemAppSettings(
                secondsBetweenReports: secondsBetweenReports ?? int.Parse(Environment.GetEnvironmentVariable("SECONDS_BETWEEN_REPORTS")!),
                timeZoneId: timeZoneId ?? Environment.GetEnvironmentVariable("TIME_ZONE")!
            )
        );
}
