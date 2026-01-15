using Serilog;

public static class LoggingExtension
{
    public static WebApplicationBuilder AddLoggingPlatform(
        this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) =>
        {
            lc.ReadFrom.Configuration(ctx.Configuration);
        });

        return builder;
    }
}