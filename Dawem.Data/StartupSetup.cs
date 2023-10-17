using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Dawem.BusinessLogic
{
    public static class StartupSetup
    {
        public static void ConfigureRepositoryContainer(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDBContext>();
            services.AddScoped<IUnitOfWork<ApplicationDBContext>, UnitOfWork<ApplicationDBContext>>();
            services.AddScoped<GeneralSetting>();
            services.AddScoped<RequestHeaderContext>();
            
        }
    }
}
