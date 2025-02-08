namespace TradingSystem.Application;

public interface ICreateInterDayReport
{
    Task Execute(CreateInterDayReportRequest createInterDayReportRequest, CancellationToken cancellationToken = default);
}
