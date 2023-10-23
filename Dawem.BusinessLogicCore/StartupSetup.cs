using Dawem.Helpers;
using Dawem.Translations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Dawem.BusinessLogic
{
    public static class StartupSetup
    {
        public static void ConfigureBusinessLogicCore(this IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
             .Where(p => p.Name.EndsWith(DawemKeys.BLC) && !p.IsInterface);

            foreach (var type in types)
            {
                var interfaceType = type.GetInterfaces(false).FirstOrDefault(i => i.Name.EndsWith(DawemKeys.BLC));
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
