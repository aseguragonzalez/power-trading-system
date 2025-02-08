namespace TradingSystem.Application;

public sealed class CreateInterDayReportRequest
{
    public readonly TimeZoneInfo TimeZone;

    public readonly DateTime ReportDate;

    public CreateInterDayReportRequest(DateTime reportDate, string timeZoneId = "Central Europe Standard Time")
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(timeZoneId));
        }

        this.ReportDate = reportDate;
        this.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
}
