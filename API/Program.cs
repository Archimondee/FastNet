
using API.Extensions;
using API.Middlewares;
using Application;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure;

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
app.UseMiddleware<GlobalExceptionMiddleware>();
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

app.Run();