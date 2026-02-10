using Application.Interface;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using FastEndpoints;
using Shared.Commons.Exceptions;

namespace Application.Auth.RegisterUser;

public sealed class RegisterUserHandler
{
  private readonly IAppDbContext _db;
  private readonly ISecurityHasher _hasher;

  public RegisterUserHandler(IAppDbContext db, ISecurityHasher hasher)
  {
    _db = db;
    _hasher = hasher;
  }

  public async Task<User> Handle(
    User user,
    CancellationToken ct)
  {
    var exists = await _db.Users
      .AnyAsync(x => x.Email == user.Email, ct);

    if (exists)
      throw new ConflictException("Email already exists");

    user.Password = _hasher.HashPassword(user.Password);

    _db.Users.Add(user);

    var roles = await _db.Roles
                  .FirstOrDefaultAsync(x => x.Name == "User", ct)
                ?? throw new InvalidOperationException("Default role not found");

    _db.UserRoles.Add(new UserRole(user.Id, roles.Id, DateTime.UtcNow, user));
    user.MarkCreated(user.Email);

    await _db.SaveChangesAsync(ct);

    var response = await _db.Users.Where(u => u.Id == user.Id)
      .Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
      .FirstOrDefaultAsync(ct);

    if (response != null) return response;

    throw new NotFoundException("User not found");
  }
}

public sealed class RegisterUserMapper
  : Mapper<RegisterUserRequest, RegisterUserResponse, User>
{
  public override User ToEntity(RegisterUserRequest r)
    => new(r.Email, r.Password, r.FirstName, r.LastName, r.IsActive);

  public override RegisterUserResponse FromEntity(User e)
    => new()
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

public sealed class RegisterUserValidator
  : AbstractValidator<RegisterUserRequest>
{
  public RegisterUserValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty()
      .EmailAddress();

    RuleFor(x => x.Password)
      .NotEmpty()
      .MinimumLength(8);

    RuleFor(x => x.FirstName)
      .NotEmpty();

    RuleFor(x => x.LastName)
      .NotEmpty();

    RuleFor(x => x.IsActive)
      .NotNull();
  }
}
