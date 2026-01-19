using System.Diagnostics;

namespace API.Middlewares;

public class PerformanceMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<PerformanceMiddleware> _logger;

  public PerformanceMiddleware(
    RequestDelegate next,
    ILogger<PerformanceMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task Invoke(HttpContext context)
  {
    var sw = Stopwatch.StartNew();

    await _next(context);

    sw.Stop();

    _logger.LogInformation(
      " HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms ",
      context.Request.Method,
      context.Request.Path,
      context.Response.StatusCode,
      sw.ElapsedMilliseconds
    );

    if (sw.ElapsedMilliseconds > 500)
    {
      _logger.LogWarning(
          "Slow HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms ",
          context.Request.Method,
          context.Request.Path,
          context.Response.StatusCode,
          sw.ElapsedMilliseconds
      );
    }
  }
}