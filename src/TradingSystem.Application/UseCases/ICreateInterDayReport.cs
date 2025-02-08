namespace TradingSystem.Application.UseCases;

public interface ICreateInterDayReport
{
    Task Execute(CreateInterDayReportRequest createInterDayReportRequest, CancellationToken cancellationToken = default);
}
