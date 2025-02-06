namespace TradingSystem.Domain;

public interface IReportRepository
{
    Task Save(Report report);
}
