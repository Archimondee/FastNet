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

      // Testing job scheduler
      var jobKey = new JobKey("exampleJob");
      q.AddJob<ExampleJob>(opts => opts.WithIdentity(jobKey));

      q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("example-trigger")
        .StartNow()
        .WithSimpleSchedule(x => x
          .WithIntervalInSeconds(10)
          .RepeatForever()));
    });
    services.Configure<QuartzOptions>(configuration.GetSection("Quartz"));
    services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });
    return services;
  }
}