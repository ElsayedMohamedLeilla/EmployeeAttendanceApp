using Dawem.Contract.BusinessLogic.Summons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;

namespace Dawem.BackgroundJobs
{
    // test CrontabSchedule in => https://crontab.cronhub.io/
    public class SubscriptionHostedService : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private IServiceScopeFactory serviceScopeFactory;

        private string Schedule => "1 * * * * *"; // Fire every one hour

        public SubscriptionHostedService(IServiceScopeFactory _serviceScopeFactory)
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
            Console.WriteLine("Handle Subscriptions " + DateTime.UtcNow.ToString("F"));
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var subscriptionBL = scope.ServiceProvider.GetRequiredService<ISubscriptionBL>();
                await subscriptionBL.HandleSubscriptions();
            }
        }
    }
}
