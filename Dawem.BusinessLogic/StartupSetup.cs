using Dawem.Helpers;
using Dawem.Translations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Dawem.BusinessLogic
{
    public static class StartupSetup
    {
        public static void ConfigureBusinessLogic(this IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
             .Where(p => p.Name.EndsWith(LeillaKeys.BL) && !p.IsInterface); 

            foreach (var type in types)
            {
                var interfaceType = type.GetInterfaces(false).FirstOrDefault(i => i.Name.EndsWith(LeillaKeys.BL));
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
