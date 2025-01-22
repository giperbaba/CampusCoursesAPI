using Quartz;

namespace repassAPI.Utils.Email;

public class RetryJobListener : IJobListener
{
    public string Name => "RetryJobListener";

    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
    {
        if (jobException != null)
        {
            
            var retryTrigger = TriggerBuilder.Create()
                .ForJob(context.JobDetail)
                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Minute)) 
                .Build();

            await context.Scheduler.ScheduleJob(retryTrigger, cancellationToken);
        }
    }
}