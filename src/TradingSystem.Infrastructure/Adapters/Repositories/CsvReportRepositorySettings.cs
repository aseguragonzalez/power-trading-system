namespace TradingSystem.Infrastructure.Adapters.Repositories;

public sealed class CsvReportRepositorySettings
{
    public readonly string Directory;

    public CsvReportRepositorySettings(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory, nameof(directory));
        Directory = directory;
    }
}
