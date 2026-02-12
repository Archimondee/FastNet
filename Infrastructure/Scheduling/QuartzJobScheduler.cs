using Application.Interface;
using Quartz;

namespace Infrastructure.Scheduling;

public sealed class QuartzJobScheduler : IJobScheduler
{
  private readonly ISchedulerFactory _schedulerFactory;

  public QuartzJobScheduler(ISchedulerFactory schedulerFactory)
  {
    _schedulerFactory = schedulerFactory;
  }

  public async Task ScheduleAsync<TJob>(
    string jobId,
    Dictionary<string, object>? data = null,
    DateTimeOffset? runAt = null)
    where TJob : IJob
  {
    var scheduler = await _schedulerFactory.GetScheduler();

    var jobKey = new JobKey(jobId);

    var job = JobBuilder.Create<TJob>()
      .WithIdentity(jobKey)
      .UsingJobData(new JobDataMap((IDictionary<string, object>)(data ?? new Dictionary<string, object>())))
      .Build();

    var trigger = TriggerBuilder.Create()
      .WithIdentity($"{jobId}-trigger")
      .ForJob(jobKey)
      .StartAt(runAt ?? DateTimeOffset.UtcNow)
      .WithSimpleSchedule(x => x
        .WithMisfireHandlingInstructionFireNow())
      .Build();

    await scheduler.ScheduleJob(
      job,
      [trigger],
      replace: true);
  }
}