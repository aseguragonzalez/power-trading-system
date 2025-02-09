namespace TradingSystem.Application.UseCases;

public sealed class CreateInterDayReportRequest
{
    public readonly TimeZoneInfo TimeZone;

    public readonly DateTime ReportDate;

    public CreateInterDayReportRequest(DateTime reportDate, TimeZoneInfo timeZoneInfo)
    {
        ArgumentNullException.ThrowIfNull(timeZoneInfo, nameof(timeZoneInfo));
        this.ReportDate = reportDate;
        this.TimeZone = timeZoneInfo;
    }
}
