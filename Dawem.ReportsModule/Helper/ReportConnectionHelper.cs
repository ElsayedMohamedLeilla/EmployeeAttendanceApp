using FastReport.Data;
using FastReport.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Dawem.ReportsModule.Helper
{

    public static class DBConnectionHelper
    {
        public static IServiceCollection AddFastReportDataConnections(this IServiceCollection services)
        {
            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
            return services;
        }
    }

}
