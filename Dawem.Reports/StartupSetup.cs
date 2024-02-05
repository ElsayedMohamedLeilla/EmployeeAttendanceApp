using Dawem.Reports.Employees.AttendanceSummaryReport;
using Microsoft.Extensions.DependencyInjection;

namespace Dawem.Reports
{
    public static class StartupSetup
    {
        public static void ConfigureReports(this IServiceCollection services)
        {
            services.AddScoped<AttendanceSummaryHelper>();
        }
    }
}
