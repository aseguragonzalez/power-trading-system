namespace TradingSystem.Application.UseCases;

public sealed class CreateInterDayReportRequest
{
    public TimeZoneInfo TimeZone { get; }

    public DateTime ReportDate { get; }

    public CreateInterDayReportRequest(DateTime reportDate, TimeZoneInfo timeZoneInfo)
    {
        ArgumentNullException.ThrowIfNull(timeZoneInfo, nameof(timeZoneInfo));
        ReportDate = reportDate;
        TimeZone = timeZoneInfo;
    }
}
