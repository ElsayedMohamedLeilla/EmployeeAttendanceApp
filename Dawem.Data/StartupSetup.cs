﻿using Dawem.Data.UnitOfWork;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Dawem.Data
{
    public static class StartupSetup
    {
        public static void ConfigureRepositoryContainer(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDBContext>();
            services.AddScoped<IUnitOfWork<ApplicationDBContext>, UnitOfWork<ApplicationDBContext>>();
            services.AddScoped<GeneralSetting>();
            services.AddScoped<RequestInfo>();

        }
    }
}
