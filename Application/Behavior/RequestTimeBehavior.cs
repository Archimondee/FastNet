using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Application.Behavior;

public class RequestTimeMiddleware
{
    private readonly RequestDelegate _next;

    public const string ElapsedKey = "__elapsed_ms";

    public RequestTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        await _next(context);

        sw.Stop();

        context.Items[ElapsedKey] = sw.ElapsedMilliseconds;
    }
}