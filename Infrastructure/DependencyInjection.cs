using Application.Interface;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<QueryInterceptor>();
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<QueryInterceptor>();
            options.UseNpgsql(
                configuration.GetConnectionString("Default"),
                npgsql =>
                {
                    npgsql.MigrationsAssembly(
                        typeof(AppDbContext).Assembly.FullName);
                });
            options.AddInterceptors(interceptor);
        });

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}