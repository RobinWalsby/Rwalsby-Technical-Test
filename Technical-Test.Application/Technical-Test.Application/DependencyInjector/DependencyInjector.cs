

using Microsoft.Extensions.DependencyInjection;
using TechnicalTest.Application.Timesheets;

namespace TechnicalTest.Application.DependencyInjector
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ITimesheetsHandlersValidationService, TimesheetsHandlersValidationService>();
            return services;
        }

    }
}
