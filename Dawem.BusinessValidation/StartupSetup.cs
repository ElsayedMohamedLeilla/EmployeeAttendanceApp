using Dawem.Translations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Dawem.Validation
{
    public static class StartupSetup
    {
        public static void ConfigureBLValidation(this IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(p => p.Name.EndsWith(LeillaKeys.BLValidation) && !p.IsInterface);

            foreach (var type in types)
            {
                var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name.EndsWith(LeillaKeys.BLValidation));
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
