using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Listener;

namespace Infrastructure.Scheduling;

public class QuartzScheduleListener : SchedulerListenerSupport
{
  private readonly ILogger<QuartzScheduleListener> _logger;

  public QuartzScheduleListener(ILogger<QuartzScheduleListener> logger)
  {
    _logger = logger;
  }

  public override Task SchedulerStarted(CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Quartz Scheduler started.");
    return Task.CompletedTask;
  }

  public override Task SchedulerError(
    string msg,
    SchedulerException cause,
    CancellationToken cancellationToken = default)
  {
    _logger.LogError(cause, "Quartz Scheduler error: {Message}", msg);
    return Task.CompletedTask;
  }

  public override Task SchedulerShutdown(CancellationToken cancellationToken = default)
  {
    _logger.LogWarning("Quartz Scheduler shutdown.");
    return Task.CompletedTask;
  }
}
