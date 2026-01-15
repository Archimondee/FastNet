
using API.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

builder.AddLoggingPlatform();
builder.Services.AddCorsExtensions(config, env);
builder.Services.AddSwaggerExtensions();
builder.Services.AddRateLimitExtension();
builder.Services.AddResponseCompressionExtension();
builder.Services.AddResponseCachingExtension();
builder.Services
    .AddFastEndpoints()
    .AddResponseCaching();


var app = builder.Build();
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