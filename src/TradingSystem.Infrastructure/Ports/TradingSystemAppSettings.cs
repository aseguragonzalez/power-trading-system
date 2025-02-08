namespace TradingSystem.Infrastructure.Ports;

public sealed class TradingSystemAppSettings
{
    public const int DefaultSecondsBetweenReports = 60;
    public const string DefaultTimeZoneId = "Europe/Madrid";

    public readonly TimeZoneInfo TimeZone;

    public readonly TimeSpan SecondsBetweenReports;

    public TradingSystemAppSettings(string timeZoneId = DefaultTimeZoneId, int secondsBetweenReports = DefaultSecondsBetweenReports)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(timeZoneId, nameof(timeZoneId));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(secondsBetweenReports, 0, nameof(secondsBetweenReports));
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.Id == timeZoneId || x.StandardName == timeZoneId)
            ?? throw new ArgumentException($"TimeZoneId ({timeZoneId}) is invalid");
        this.TimeZone = timeZoneInfo!;
        this.SecondsBetweenReports = TimeSpan.FromSeconds(secondsBetweenReports);
    }
}
