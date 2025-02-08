using System.Globalization;
using TradingSystem.Domain;

namespace TradingSystem.Infrastructure.Adapters.Repositories;

public sealed class CsvReportRepository : IReportRepository
{
    private readonly CsvReportRepositorySettings settings;

    public CsvReportRepository(CsvReportRepositorySettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings, nameof(settings));

        this.settings = settings;
    }

    public async Task Save(Report report)
    {
        ArgumentNullException.ThrowIfNull(report, nameof(report));

        if (!Directory.Exists(settings.Directory))
        {
            Directory.CreateDirectory(settings.Directory);
        }

        using FileStream csvReportStream = File.OpenWrite(Path.Combine(settings.Directory, report.ReportName));
        await using StreamWriter writer = new(csvReportStream);
        await writer.WriteLineAsync("Datetime;Volume");
        foreach (ReportPosition position in report.GetPositions())
        {
            string datetime = position.Slot.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            string volume = position.Volume.ToString("F2", CultureInfo.InvariantCulture);
            await writer.WriteLineAsync($"{datetime};{volume}");
        }
    }
}
