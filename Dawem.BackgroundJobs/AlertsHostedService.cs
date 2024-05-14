using Dawem.Contract.BusinessLogic.Dawem.Summons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;

namespace Dawem.BackgroundJobs
{
    // test CrontabSchedule in => https://crontab.cronhub.io/
    public class AlertsHostedService : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private IServiceScopeFactory serviceScopeFactory;

        private string Schedule => "* */1 * * * *"; // Fire every 1 minute

        public AlertsHostedService(IServiceScopeFactory _serviceScopeFactory)
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
            Console.WriteLine("Handle Alerts" + DateTime.UtcNow.ToString("F"));
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var summonBL = scope.ServiceProvider.GetRequiredService<ISummonBL>();
                await summonBL.HandleSummonLog();
            }
        }
    }
}
