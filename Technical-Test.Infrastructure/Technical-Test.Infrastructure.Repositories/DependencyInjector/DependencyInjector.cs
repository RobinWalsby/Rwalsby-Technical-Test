

using Microsoft.Extensions.DependencyInjection;
using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Infrastructure.Repositories;

namespace Technical_Test.Infrastructure.DependencyInjector
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ITimesheetRepository, TimesheetRepository>();
            return services;
        }

    }
}
