using Application.Common.Listing;
using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetUsers;

public class GetUserHandler
{
  private readonly IAppDbContext _db;
  private readonly IListQueryProcessor<User> _processor;

  public GetUserHandler(IAppDbContext db, IListQueryProcessor<User> processor)
  {
    _db = db;
    _processor = processor;
  }

  public async Task<PagedResult<GetUserResponse>> Handle(ListQuery req, CancellationToken ct)
  {
    var query = _db.Users
      .AsNoTracking()
      .Include(x => x.UserRoles)
      .Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
      .ThenInclude(r => r.RolePermissions)
      .ThenInclude(rp => rp.Permission);

    var result = await _processor.ExecuteAsync(query, req, ct);
    var mapped = result.Items
      .Select(Map)
      .ToList();

    return new PagedResult<GetUserResponse>(
      mapped,
      result.TotalCount,
      result.Page,
      result.PageSize);
  }

  private static GetUserResponse Map(User user)
  {
    return new GetUserResponse
    {
      Id = user.Id,
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
      IsActive = user.IsActive,
      Roles = user.UserRoles.Select(x => new RoleResponse
      {
        Id = x.RoleId,
        Name = x.Role.Name,
        Permissions = x.Role.RolePermissions.Select(p => p.Permission.Name).ToArray()
      }).ToArray(),
      CreatedAt = user.CreatedAt,
      UpdatedAt = user.UpdatedAt,
      CreatedBy = user.CreatedBy,
      UpdatedBy = user.UpdatedBy,
    };
  }
}