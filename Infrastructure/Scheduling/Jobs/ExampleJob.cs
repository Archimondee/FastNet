using Quartz;

namespace Infrastructure.Scheduling.Jobs;

public class ExampleJob : IJob
{
  public Task Execute(IJobExecutionContext context)
  {
    Console.WriteLine($"Hello from Quartz! {DateTime.Now}");
    return Task.CompletedTask;
  }
}