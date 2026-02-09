using Application.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class PermissionService
{
  private readonly IAppDbContext _db;
  private readonly ILogger<PermissionService> _logger;

  public PermissionService(IAppDbContext db, ILogger<PermissionService> logger)
  {
    _db = db;
    _logger = logger;
  }

  public async Task<bool> HasPermissionsAsync(
    IEnumerable<string> roles,
    IEnumerable<string> requiredPermissions,
    CancellationToken ct = default)
  {
    var rolesList = roles.ToList();
    _logger.LogInformation("Checking permissions for roles: {Roles}", string.Join(", ", rolesList));

    var permissions = await _db.Roles
      .Where(r => rolesList.Contains(r.Name))
      .SelectMany(r => r.RolePermissions)
      .Select(rp => rp.Permission.Name)
      .Distinct()
      .ToListAsync(ct);

    _logger.LogInformation("Found {Count} permissions: {Permissions}", permissions.Count,
      string.Join(", ", permissions));

    var result = requiredPermissions.All(p => permissions.Contains(p));
    _logger.LogInformation("Permission check result: {Result}", result);

    return result;
  }
}
