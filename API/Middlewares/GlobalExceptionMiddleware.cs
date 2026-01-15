using System.Text.Json;
using FluentValidation;
using Serilog;
using Shared.Commons.Error;
using Shared.Commons.Response;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var correlationId =
            context.Items["X-Correlation-Id"]?.ToString()
            ?? context.TraceIdentifier;

        context.Response.ContentType = "application/json";

        // =========================
        // 1️⃣ FluentValidation (400)
        // =========================
        if (exception is ValidationException ve)
        {
            Log.Warning(
                "Validation failed | {Endpoint} | {ErrorCount} | {CorrelationId}",
                context.Request.Path,
                ve.Errors.Count(),
                correlationId);

            context.Response.StatusCode = 400;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new
                {
                    code = ErrorCodes.ValidationError,
                    message = "Validation failed",
                    errors = ve.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    }),
                    correlationId
                }));

            return;
        }

        // =========================
        // 2️⃣ Application Exceptions
        // =========================
        if (exception is AppException appEx)
        {
            Log.Warning(
                "Application error | {Code} | {StatusCode} | {Endpoint} | {CorrelationId}",
                appEx.Code,
                appEx.StatusCode,
                context.Request.Path,
                correlationId);

            context.Response.StatusCode = appEx.StatusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new ApiErrorResponse
                {
                    Code = appEx.Code,
                    Message = appEx.Message,
                    CorrelationId = correlationId
                }));

            return;
        }

        // =========================
        // 3️⃣ Unknown / System Errors
        // =========================
        Log.Error(
            exception,
            "Unhandled system exception | {Endpoint} | {CorrelationId}",
            context.Request.Path,
            correlationId);

        context.Response.StatusCode = 500;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new ApiErrorResponse
            {
                Code = ErrorCodes.InternalServerError,
                Message = _env.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred",
                CorrelationId = correlationId
            }));
    }
}
