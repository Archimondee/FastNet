using System.Collections.Concurrent;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public sealed class QueryInterceptor : DbCommandInterceptor
{
  private const int SlowQueryThresholdMs = 500;
  private static readonly ConcurrentDictionary<Guid, Stopwatch> Timers = new();

  private readonly ILogger<QueryInterceptor> _logger;

  public QueryInterceptor(ILogger<QueryInterceptor> logger)
  {
    _logger = logger;
  }

  private static void Start(Guid commandId)
    => Timers[commandId] = Stopwatch.StartNew();

  private void StopAndLog(DbCommand command, CommandExecutedEventData eventData)
  {
    if (!Timers.TryRemove(eventData.CommandId, out var sw))
      return;

    sw.Stop();
    var elapsed = sw.ElapsedMilliseconds;

    if (elapsed > SlowQueryThresholdMs)
    {
      _logger.LogWarning(
        "SLOW SQL ({ElapsedMs}ms)\n{Sql}",
        elapsed,
        command.CommandText);
    }
    else
    {
      _logger.LogInformation(
        "SQL ({ElapsedMs}ms)\n{Sql}",
        elapsed,
        command.CommandText);
    }
  }

  public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
    DbCommand command,
    CommandEventData eventData,
    InterceptionResult<DbDataReader> result,
    CancellationToken cancellationToken = default)
  {
    Start(eventData.CommandId);
    return ValueTask.FromResult(result);
  }

  public override ValueTask<DbDataReader> ReaderExecutedAsync(
    DbCommand command,
    CommandExecutedEventData eventData,
    DbDataReader result,
    CancellationToken cancellationToken = default)
  {
    StopAndLog(command, eventData);
    return ValueTask.FromResult(result);
  }

  public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
    DbCommand command,
    CommandEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
  {
    Start(eventData.CommandId);
    return ValueTask.FromResult(result);
  }

  public override ValueTask<int> NonQueryExecutedAsync(
    DbCommand command,
    CommandExecutedEventData eventData,
    int result,
    CancellationToken cancellationToken = default)
  {
    StopAndLog(command, eventData);
    return ValueTask.FromResult(result);
  }

  public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(
    DbCommand command,
    CommandEventData eventData,
    InterceptionResult<object> result,
    CancellationToken cancellationToken = default)
  {
    Start(eventData.CommandId);
    return ValueTask.FromResult(result);
  }

  public override ValueTask<object?> ScalarExecutedAsync(
    DbCommand command,
    CommandExecutedEventData eventData,
    object result,
    CancellationToken cancellationToken = default)
  {
    StopAndLog(command, eventData);
    return ValueTask.FromResult(result)!;
  }
}
