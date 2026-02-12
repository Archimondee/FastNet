using Quartz;

namespace Application.Interface;

public interface IJobScheduler
{
  Task ScheduleAsync<TJob>(
    string jobId,
    Dictionary<string, object>? data = null,
    DateTimeOffset? runAt = null)
    where TJob : IJob;
}