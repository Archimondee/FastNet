namespace API.Extensions;

public static class LogEnrichmentExtensions
{
    public static IDisposable EnrichRequest(
        HttpContext context)
    {
        return Serilog.Context.LogContext.Push(
            new Serilog.Core.Enrichers.PropertyEnricher(
                "ClientIP",
                context.Connection.RemoteIpAddress?.ToString()),

            new Serilog.Core.Enrichers.PropertyEnricher(
                "Path",
                context.Request.Path)
        );
    }
}
