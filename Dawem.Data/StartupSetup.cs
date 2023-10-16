using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Dawem.BusinessLogic
{
    public static class StartupSetup
    {
        public static void ConfigureRepositoryContainer(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDBContext>();
            services.AddScoped<IUnitOfWork<ApplicationDBContext>, UnitOfWork<ApplicationDBContext>>();

        }
    }
}
