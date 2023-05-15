using Hangfire;
using InitialWorkerService.Services;

namespace InitialWorkerService.Jobs
{
    public abstract class JobScheduler
    {
        public static void ScheduleRecurringJob()
        {
            RecurringJob.AddOrUpdate<IWorkService>("message-logging", item => item.DoWork(), Cron.Minutely);
            RecurringJob.AddOrUpdate<IWorkService>("message-logging-monthly", item => item.DoWork(), Cron.Monthly(18));
        }
    }
}
