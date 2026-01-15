namespace API.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsExtensions(
        this IServiceCollection services,
        IConfiguration config, IHostEnvironment env)
    {
        services.AddCors(options =>
        {
            if (env.EnvironmentName is "Development" or "Staging")
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }
            else
            {
                options.AddDefaultPolicy(policy =>
                {
                    var origins = config
                        .GetSection("Cors:Origins")
                        .Get<string[]>() ?? [];

                    policy
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            }
        });

        return services;
    }

}
