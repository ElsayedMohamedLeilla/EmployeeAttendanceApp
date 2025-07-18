﻿using Dawem.Contract.Repository.Manager;
using Dawem.Helpers;
using Dawem.Repository.Managers;
using Dawem.Translations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Dawem.Repository
{
    public static class StartupSetup
    {

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            var types = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(p => p.Name.EndsWith(LeillaKeys.Repository) && !p.IsInterface);

            foreach (var type in types)
            {
                var interfaceType = type.GetInterfaces(false).FirstOrDefault(i => i.Name.EndsWith(LeillaKeys.Repository));
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
                else
                {
                    services.AddScoped(type);
                }
            }

        }

    }
}
