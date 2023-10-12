using Dawem.Translations;
using Glamatek.Contract.Repository.RepositoryManager;
using Glamatek.Repository.Revamp_PhaseOne.RepositoryManager;
using Glamatek.Utils.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Glamatek.Repository
{
    public static class StartupSetup
    {

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            var types = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(p => p.Name.EndsWith(DawemKeys.Repository) && !p.IsInterface);

            foreach (var type in types)
            {
                var interfaceType = type.GetInterfaces(false).FirstOrDefault(i => i.Name.EndsWith(DawemKeys.Repository));
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
