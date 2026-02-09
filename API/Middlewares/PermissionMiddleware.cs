using System.Security.Claims;
using Application.Services;
using Shared.Commons.Response;

namespace API.Middlewares;

public sealed class PermissionMiddleware : IMiddleware
{
  private readonly PermissionService _evaluator;

  public PermissionMiddleware(
    PermissionService evaluator)
  {
    _evaluator = evaluator;
  }

  public async Task InvokeAsync(
    HttpContext context,
    RequestDelegate next)
  {
    var endpoint = context.GetEndpoint();
    var requirements = endpoint?
      .Metadata
      .GetOrderedMetadata<RequirePermissionAttribute>();

    if (requirements == null || requirements.Count == 0)
    {
      await next(context);
      return;
    }

    if (!context.User.Identity?.IsAuthenticated ?? true)
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      return;
    }

    var roles = context.User.Claims
      .Where(c => c.Type is ClaimTypes.Role or "role")
      .Select(c => c.Value)
      .Distinct()
      .ToList();

    var requiredPermissions = requirements.Select(r => r.Permission).ToList();

    var allowed = await _evaluator.HasPermissionsAsync(
      roles,
      requiredPermissions
    );

    if (!allowed)
    {
      context.Response.StatusCode = StatusCodes.Status403Forbidden;
      await context.Response.WriteAsJsonAsync(new ApiErrorResponse
      {
        Code = "FORBIDDEN",
        Message = "You don't have permission to access this resource",
        CorrelationId = context.Items["X-Correlation-Id"]?.ToString() ?? context.TraceIdentifier
      });
      return;
    }

    await next(context);
  }
}