using Application.Interface;
using Domain.Entities;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Exceptions;

namespace Application.Users.GetMe;

public class GetMeHandler
{
  private readonly IAppDbContext _db;

  public GetMeHandler(IAppDbContext db)
  {
    _db = db;
  }

  public async Task<User> Handle(string userId, CancellationToken ct)
  {
    if (Guid.TryParse(userId, out var id))
    {
      var user = await _db.Users
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .ThenInclude(r => r.RolePermissions)
        .ThenInclude(rp => rp.Permission)
        .FirstOrDefaultAsync(x => x.Id == id, ct);

      return user ?? throw new NotFoundException("User not found");
    }

    throw new NotFoundException("User not found");
  }
}

public sealed class GetMeMapper : Mapper<EmptyRequest, GetMeResponse, User>
{
  public override GetMeResponse FromEntity(User e) =>
    new()
    {
      Id = e.Id,
      Email = e.Email,
      FirstName = e.FirstName,
      LastName = e.LastName,
      IsActive = e.IsActive,
      Roles = e.UserRoles.Select(ur => new RoleResponse
      {
        Id = ur.RoleId,
        Name = ur.Role.Name,
        Permissions = ur.Role.RolePermissions.Select(rp => rp.Permission.Name).ToArray()
      }).ToArray(),
      CreatedAt = e.CreatedAt,
      UpdatedAt = e.UpdatedAt,
      CreatedBy = e.CreatedBy,
      UpdatedBy = e.UpdatedBy,
    };
}