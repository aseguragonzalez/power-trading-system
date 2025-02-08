
using TradingSystem.Application.UseCases;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services) =>
            services.AddSingleton<ICreateInterDayReport, CreateInterDayReport>();
    }
}
