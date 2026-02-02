using System.Text.Json;
using Application.Interface;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class DatabaseInitializers
{
  private readonly AppDbContext _context;
  private readonly ISecurityHasher _hasher;
  private readonly IWebHostEnvironment _env;

  public DatabaseInitializers(
    AppDbContext context,
    ISecurityHasher hasher,
    IWebHostEnvironment env)
  {
    _context = context;
    _hasher = hasher;
    _env = env;
  }

  public async Task SeedAsync()
  {
    await _context.Database.MigrateAsync();

    if (await _context.Users.AnyAsync())
    {
      return;
    }

    var seedPath = Path.Combine(_env.ContentRootPath, "Seed");

    var users = Read<List<UserSeedDto>>(Path.Combine(seedPath, "users.json"))
      .Select(u => new User(u.Email, _hasher.HashPassword(u.Password), u.FirstName, u.LastName, u.IsActive)).ToList();

    var permissions = Read<List<PermissionSeedDto>>(Path.Combine(seedPath, "permissions.json"))
      .Select(p => new Permission(p.Name, p.Description)).ToList();

    var roles = Read<List<RoleSeedDto>>(Path.Combine(seedPath, "roles.json"))
      .Select(r => new Role(r.Name, r.Description)).ToList();

    var roleMap = roles.ToDictionary(r => r.Name);
    var permissionMap = permissions.ToDictionary(p => p.Name);

    var rolePermissions = Read<List<RolePermissionSeedDto>>(Path.Combine(seedPath, "role-permissions.json"))
      .SelectMany(rp =>
      {
        var role = roleMap[rp.Role];
        var perms = rp.Permissions.Contains("*")
          ? permissionMap.Values
          : rp.Permissions.Select(p => permissionMap[p]);
        return perms.Select(p =>
          new RolePermission(role.Id, p.Id)
        );
      }).ToList();
    var userMap = users.ToDictionary(u => u.Email);
    var userRoles = Read<List<UserSeedDto>>(
      Path.Combine(seedPath, "users.json")
    ).Select(dto =>
    {
      if (!userMap.TryGetValue(dto.Email, out var user))
        throw new InvalidOperationException($"User '{dto.Email}' not found");

      if (!roleMap.TryGetValue(dto.Role, out var role))
        throw new InvalidOperationException($"Role '{dto.Role}' not found");

      return new UserRole(
        user.Id,
        role.Id,
        DateTime.UtcNow
      );
    }).ToList();

    _context.Permissions.AddRange(permissions);
    _context.Roles.AddRange(roles);
    await _context.SaveChangesAsync();

    _context.Users.AddRange(users);
    await _context.SaveChangesAsync();

    _context.RolePermissions.AddRange(rolePermissions);
    _context.UserRoles.AddRange(userRoles);
    await _context.SaveChangesAsync();
  }

  private static T Read<T>(string path)
  {
    var json = File.ReadAllText(path);
    return JsonSerializer.Deserialize<T>(json)!;
  }
}