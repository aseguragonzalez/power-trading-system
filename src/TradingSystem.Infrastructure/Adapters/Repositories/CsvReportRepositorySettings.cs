namespace TradingSystem.Infrastructure.Adapters.Repositories;

public sealed class CsvReportRepositorySettings
{
    public string Directory { get; }

    public CsvReportRepositorySettings(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory, nameof(directory));
        Directory = directory;
    }
}
