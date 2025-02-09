using TradingSystem.Domain.Entities;

namespace TradingSystem.Domain.Repositories;

public interface IReportRepository
{
    Task Save(Report report, CancellationToken cancellationToken = default);
}
