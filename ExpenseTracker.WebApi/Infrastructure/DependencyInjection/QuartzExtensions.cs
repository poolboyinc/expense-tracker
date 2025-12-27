using Quartz;

namespace ExpenseTracker.WebApi.Infrastructure.DependencyInjection
{
    public static class QuartzExtensions
    {
        public static void AddJobAndTrigger<T>(
            this IServiceCollectionQuartzConfigurator q,
            string cronExpression) where T : IJob
        {
            string jobName = typeof(T).Name;
            var jobKey = new JobKey(jobName);

            q.AddJob<T>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-Trigger")
                .WithCronSchedule(cronExpression));
        }
    }
}
