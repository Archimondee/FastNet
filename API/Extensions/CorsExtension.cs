namespace API.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsExtensions(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("default", policy =>
            {
                policy
                    .WithOrigins(config.GetSection("Cors:Origins").Get<string[]>()!)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
