using Application.Interface;
using FastEndpoints.Security;
using Infrastructure.Auth;
using Infrastructure.Email;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Query;
using Infrastructure.Scheduling;
using Infrastructure.Security;
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

        services.Configure<JwtSettings>(
            configuration.GetSection("Jwt")
        );

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<ISecurityHasher, BCryptPasswordHasher>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<DatabaseInitializers>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddAuthenticationJwtBearer(o =>
        {
            o.SigningKey = configuration.GetSection("Jwt").Get<JwtSettings>()!.Secret;
        });

        services.AddAuthentication();
        services.AddAuthorization();
        services.AddSingleton(sp =>
            configuration.GetSection("Email:Smtp").Get<SmtpOptions>()!);

        services.AddScoped(typeof(IListQueryProcessor<>), typeof(EfListQueryProcessor<>));
        services.AddQuartzConfiguration(configuration);
        services.AddScoped<IJobScheduler, QuartzJobScheduler>();
        services.AddSingleton(new RateLimiter(
            limit: 20,
            interval: TimeSpan.FromMinutes(1)));
        services.AddScoped<IEmailTemplateRenderer, TemplateRenderer>();
        services.AddScoped<IEmailSender, EmailSmtpServices>();

        return services;
    }
}