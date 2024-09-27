using Dawem.Contract.BusinessLogic.Dawem.Schedules.SchedulePlans;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;

namespace Dawem.BackgroundJobs
{
    // test CrontabSchedule in => https://crontab.cronhub.io/
    public class SchedulePlanHostedService : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private IServiceScopeFactory serviceScopeFactory;

        //private string Schedule => "0 10 0 * * *"; // Fire at 12:10 am every day
        private string Schedule => "0 0 */1 * * *"; // Fire every one hour
        //private string Schedule => "*/10 * * * * *"; // Runs every 10 sec

        public SchedulePlanHostedService(IServiceScopeFactory _serviceScopeFactory)
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
            serviceScopeFactory = _serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {        
                if (DateTime.UtcNow > _nextRun)
                {
                    await Process();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private async Task Process()
        {
            Console.WriteLine("Handle Schedule Plans " + DateTime.UtcNow.ToString("F"));
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var schedulePlanBL = scope.ServiceProvider.GetRequiredService<ISchedulePlanBL>();
                await schedulePlanBL.HandleSchedulePlans();

            }
        }
    }
}
