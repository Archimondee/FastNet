
using API.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services
    .AddFastEndpoints()
    .AddResponseCaching();

builder.Services.AddCorsExtensions(config);
builder.Services.AddSwaggerExtensions();

var app = builder.Build();
app.UseResponseCaching()
    .UseFastEndpoints();
app.UseSwaggerGen();

app.Run();