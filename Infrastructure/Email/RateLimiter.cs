namespace Infrastructure.Email;

public sealed class RateLimiter
{
  private readonly int _limit;
  private readonly TimeSpan _interval;
  private readonly Queue<DateTime> _timestamps = new();
  private readonly object _lock = new();

  public RateLimiter(int limit, TimeSpan interval)
  {
    _limit = limit;
    _interval = interval;
  }

  public async Task WaitAsync(CancellationToken ct = default)
  {
    while (true)
    {
      ct.ThrowIfCancellationRequested();

      TimeSpan? delay;

      lock (_lock)
      {
        var now = DateTime.UtcNow;

        while (_timestamps.Count > 0 &&
               now - _timestamps.Peek() > _interval)
        {
          _timestamps.Dequeue();
        }

        if (_timestamps.Count < _limit)
        {
          _timestamps.Enqueue(now);
          return;
        }

        delay = _interval - (now - _timestamps.Peek());
      }

      if (delay.HasValue)
        await Task.Delay(delay.Value, ct);
    }
  }
}