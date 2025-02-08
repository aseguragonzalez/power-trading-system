using System.Globalization;
using TradingSystem.Domain.Entities;
using TradingSystem.Domain.Repositories;

namespace TradingSystem.Infrastructure.Adapters.Repositories;

public sealed class CsvReportRepository : IReportRepository
{
    private const string CsvHeaders = "Datetime;Volume";
    private const string ISO8610DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
    private const string DoubleFormat = "F2";

    private readonly CsvReportRepositorySettings settings;

    public CsvReportRepository(CsvReportRepositorySettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings, nameof(settings));
        this.settings = settings;
    }

    public async Task Save(Report report, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(report, nameof(report));

        if (!Directory.Exists(settings.Directory))
        {
            Directory.CreateDirectory(settings.Directory);
        }

        cancellationToken.ThrowIfCancellationRequested();
        using FileStream csvReportStream = File.OpenWrite(Path.Combine(settings.Directory, report.ReportName));
        await using StreamWriter writer = new(csvReportStream);
        await writer.WriteLineAsync(CsvHeaders);
        foreach (ReportPosition position in report.GetPositions())
        {

            string datetime = position.Period.ToString(ISO8610DateTimeFormat, CultureInfo.InvariantCulture);
            string volume = position.Volume.ToString(DoubleFormat, CultureInfo.InvariantCulture);
            await writer.WriteLineAsync($"{datetime};{volume}");
        }
    }
}
