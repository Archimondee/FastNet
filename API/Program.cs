using API.Extensions;
using API.Middlewares;
using Application;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

builder.AddLoggingPlatform();
builder.Services.AddCorsExtensions(config, env);
builder.Services.AddSwaggerExtensions();
builder.Services.AddRateLimitExtension();
builder.Services.AddResponseCompressionExtension();
builder.Services.AddResponseCachingExtension();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(config);

builder.Services
    .AddFastEndpoints()
    .AddResponseCaching();

var app = builder.Build();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSecurityHeaders();
app.UseLoggingPlatform();
app.UseCors();
app.UseRateLimiter();
app.UseResponseCaching();
app.UseResponseCompression();
app.UseFastEndpoints();
app.UseSwaggerGen();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var server = app.Services.GetRequiredService<IServer>();
    var addresses = server.Features.Get<IServerAddressesFeature>();

    if (addresses is not null)
    {
        foreach (var address in addresses.Addresses)
        {
            Log.Information("Listening on {Address}", address);
        }
    }
});

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var initializer = scope.ServiceProvider
        .GetRequiredService<DatabaseInitializers>();

    await initializer.SeedAsync();
}

app.Run();