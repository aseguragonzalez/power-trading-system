namespace TradingSystem.Application;

public sealed class CreateInterDayReportRequest
{
    public readonly DateTime ReportDate;

    public CreateInterDayReportRequest(DateTime reportDate)
    {
        ReportDate = reportDate;
    }
}
