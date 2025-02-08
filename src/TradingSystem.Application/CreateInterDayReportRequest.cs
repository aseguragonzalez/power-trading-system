namespace TradingSystem.Application;

public sealed class CreateInterDayReportRequest
{
    public readonly TimeZoneInfo TimeZone;

    public readonly DateTime ReportDate;

    public CreateInterDayReportRequest(DateTime reportDate, string timeZoneId = "Central Europe Standard Time")
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(timeZoneId, nameof(timeZoneId));
        this.ReportDate = reportDate;
        this.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
}
