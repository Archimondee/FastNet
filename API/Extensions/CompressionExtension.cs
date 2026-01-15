using Microsoft.AspNetCore.ResponseCompression;

namespace API.Extensions;

public static class CompressionExtensions
{
    public static IServiceCollection AddResponseCompressionExtension(
        this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });

        services.Configure<GzipCompressionProviderOptions>(o =>
        {
            o.Level = System.IO.Compression.CompressionLevel.Fastest;
        });

        services.Configure<BrotliCompressionProviderOptions>(o =>
        {
            o.Level = System.IO.Compression.CompressionLevel.Fastest;
        });

        return services;
    }
}