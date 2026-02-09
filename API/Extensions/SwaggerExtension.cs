using FastEndpoints.Swagger;
using NSwag;

namespace API.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerExtensions(this IServiceCollection services)
    {
        services.SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "Fast API";
                s.Version = "v1";
            };
        });

        return services;
    }
}