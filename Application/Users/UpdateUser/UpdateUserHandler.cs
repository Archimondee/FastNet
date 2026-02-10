using Application.Interface;
using Domain.Entities;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Exceptions;

namespace Application.Users.UpdateUser;

public sealed class UpdateUserHandler
{
  private readonly IAppDbContext _db;
  private readonly ISecurityHasher _hasher;

  public UpdateUserHandler(IAppDbContext db, ISecurityHasher hasher)
  {
    _db = db;
    _hasher = hasher;
  }

  public async Task<User> Handle(
    Guid userId,
    User req,
    CancellationToken ct)
  {
    var user = await _db.Users
      .Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
      .FirstOrDefaultAsync(x => x.Id == userId, ct);

    if (user is null)
      throw new NotFoundException("User not found");

    if (!string.IsNullOrWhiteSpace(req.Email))
      user.Email = req.Email;

    if (!string.IsNullOrWhiteSpace(req.Password))
      user.Password = _hasher.HashPassword(req.Password);

    if (!string.IsNullOrWhiteSpace(req.FirstName))
      user.FirstName = req.FirstName;

    if (!string.IsNullOrWhiteSpace(req.LastName))
      user.LastName = req.LastName;

    user.MarkUpdated();

    await _db.SaveChangesAsync(ct);

    return user;
  }
}

public sealed class UpdateUserMapper : Mapper<UpdateUserRequest, UpdateUserResponse, User>
{
  public override User ToEntity(UpdateUserRequest r) =>
    new(r.Email ?? "", r.Password ?? "", r.FirstName ?? "", r.LastName ?? "", true);

  public override UpdateUserResponse FromEntity(User e) =>
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
        Name = ur.Role.Name
      }).ToArray(),
      CreatedAt = e.CreatedAt,
      UpdatedAt = e.UpdatedAt,
      CreatedBy = e.CreatedBy,
      UpdatedBy = e.UpdatedBy,
    };
}

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
  public UpdateUserValidator()
  {
    When(x => x.Email != null, () => { RuleFor(x => x.Email).NotEmpty().EmailAddress(); });
    When(x => x.Password != null, () => { RuleFor(x => x.Password).NotEmpty().MinimumLength(8); });
    When(x => x.FirstName != null, () => { RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50); });
    When(x => x.LastName != null, () => { RuleFor(x => x.LastName).NotEmpty().MaximumLength(50); });
  }
}