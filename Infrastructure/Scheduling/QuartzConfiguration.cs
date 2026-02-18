using Infrastructure.Scheduling.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure.Scheduling;

public static class QuartzConfiguration
{
  public static IServiceCollection AddQuartzConfiguration(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    var jobConfigs = configuration
      .GetSection("Quartz:QuartzJobs")
      .Get<List<QuartzJobConfig>>() ?? new();

    var jobTypeMap = new Dictionary<QuartzJobType, Type>
    {
      { QuartzJobType.ExampleJob, typeof(ExampleJob) },
      { QuartzJobType.EmailDispatcherJob, typeof(EmailDispatcherJob) }
    };

    services.AddQuartz(q =>
    {
      q.UseMicrosoftDependencyInjectionJobFactory();
      q.UsePersistentStore(s =>
      {
        s.UseProperties = true;
        s.UseJsonSerializer();
      });
      q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 5; });
      q.AddSchedulerListener<QuartzScheduleListener>();
      q.AddJobListener<QuartzJobListener>();

      foreach (var jobConfig in jobConfigs.Where(x => x.Enabled))
      {
        if (!jobTypeMap.TryGetValue(jobConfig.Type, out var jobType))
          continue;

        var jobKey = new JobKey(jobType.Name);

        q.AddJob(jobType, jobKey, _ => { });

        q.AddTrigger(trigger => trigger
          .ForJob(jobKey)
          .WithIdentity($"{jobKey.Name}-trigger")
          .WithCronSchedule(jobConfig.Cron));
      }
    });
    services.Configure<QuartzOptions>(configuration.GetSection("Quartz"));
    services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });
    return services;
  }
}

public enum QuartzJobType
{
  ExampleJob,
  EmailDispatcherJob
}

public class QuartzJobConfig
{
  public QuartzJobType Type { get; set; }

  public string Cron { get; set; } = "0 */5 * * * ?";

  public bool Enabled { get; set; } = true;
}