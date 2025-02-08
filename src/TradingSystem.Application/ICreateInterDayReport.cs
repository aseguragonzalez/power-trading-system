namespace TradingSystem.Application;

public interface ICreateInterDayReport
{
    Task Execute(CreateInterDayReportRequest request);
}
