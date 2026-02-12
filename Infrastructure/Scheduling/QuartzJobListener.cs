using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Listener;

namespace Infrastructure.Scheduling;

public class QuartzJobListener : JobListenerSupport
{
  private readonly ILogger<QuartzJobListener> _logger;

  public QuartzJobListener(ILogger<QuartzJobListener> logger)
  {
    _logger = logger;
  }

  public override string Name => "GlobalJobListener";

  public override Task JobWasExecuted(
    IJobExecutionContext context,
    JobExecutionException? jobException,
    CancellationToken cancellationToken = default)
  {
    var jobName = context.JobDetail.Key.Name;
    var jobGroup = context.JobDetail.Key.Group;

    if (jobException != null)
    {
      _logger.LogError(
        jobException,
        "Quartz Job {JobGroup}.{JobName} failed.",
        jobGroup,
        jobName);
    }
    else
    {
      _logger.LogInformation(
        "Quartz Job {JobGroup}.{JobName} executed successfully.",
        jobGroup,
        jobName);
    }

    return Task.CompletedTask;
  }
}
