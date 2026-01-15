using API.Middlewares;

namespace API.Extensions;

public static class LoggingMiddlewareExtension
{
    public static WebApplication UseLoggingPlatform(
        this WebApplication app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();

        app.Use(async (ctx, next) =>
        {
            using (LogEnrichmentExtensions.EnrichRequest(ctx))
            {
                await next();
            }
        });

        return app;
    }
}