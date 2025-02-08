namespace TradingSystem.Infrastructure.Ports;

public sealed class TradingSystemAppSettings
{
    public const int DefaultTimeBewteenReports = 60;
    public const string DefaultTimeZoneId = "Central European Standard Time";

    public readonly string TimeZoneId;

    public readonly int TimeBewteenReportsInSeconds;

    public TradingSystemAppSettings(string timeZoneId = DefaultTimeZoneId, int timeBewteenReportsInSeconds = DefaultTimeBewteenReports)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(timeZoneId, nameof(timeZoneId));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual<int>(timeBewteenReportsInSeconds, 0, nameof(timeBewteenReportsInSeconds));
        if (!TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == timeZoneId || x.StandardName == timeZoneId))
        {
            throw new ArgumentException($"TimeZoneId ({timeZoneId}) is invalid");
        }

        this.TimeZoneId = timeZoneId;
        this.TimeBewteenReportsInSeconds = timeBewteenReportsInSeconds;
    }
}
