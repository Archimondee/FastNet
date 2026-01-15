namespace API.Extensions;

public static class HeadersExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<SecurityHeadersMiddleware>();
        return app;
    }
}

public sealed class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var headers = context.Response.Headers;

        headers["X-Content-Type-Options"] = "nosniff";
        headers["X-XSS-Protection"] = "1; mode=block";
        headers["Referrer-Policy"] = "no-referrer";
        headers["Content-Security-Policy"] =
            "default-src 'self'; frame-ancestors 'none';";

        await _next(context);
    }
}
