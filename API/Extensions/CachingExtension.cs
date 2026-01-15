namespace API.Extensions;

public static class CachingExtensions
{
    public static IServiceCollection AddResponseCachingExtension(
        this IServiceCollection services)
    {
        services.AddResponseCaching();
        return services;
    }
}