using Application.Interface;
using Domain.Entities;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Exceptions;

namespace Application.Auth.LoginUser;

public sealed class LoginUserHandler
{
  private readonly IAppDbContext _db;
  private readonly ISecurityHasher _hasher;
  private readonly IJwtTokenGenerator _jwtTokenGenerator;

  public LoginUserHandler(
    IAppDbContext db,
    ISecurityHasher hasher,
    IJwtTokenGenerator jwtTokenGenerator)
  {
    _db = db;
    _hasher = hasher;
    _jwtTokenGenerator = jwtTokenGenerator;
  }

  public async Task<LoginUserResponse> Handle(
    User user,
    CancellationToken ct)
  {
    var exists = await _db.Users
      .AnyAsync(x => x.Email == user.Email, ct);

    if (exists)
    {
      var userData = await _db.Users
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .ThenInclude(pr => pr.RolePermissions)
        .ThenInclude(pr => pr.Permission)
        .FirstOrDefaultAsync(x => x.Email == user.Email, ct);

      if (userData == null)
        throw new NotFoundException("User not found");

      if (!_hasher.VerifyPassword(userData.Password, user.Password))
        throw new UnauthorizedException("Wrong Email or Password");

      var accessToken = _jwtTokenGenerator.GenerateToken(userData);

      return new LoginUserResponse
      {
        AccessToken = accessToken,

        Id = userData.Id,
        Email = userData.Email,
        FirstName = userData.FirstName,
        LastName = userData.LastName,
        IsActive = userData.IsActive,
        Roles = userData.UserRoles.Select(ur => new RoleResponse
        {
          Id = ur.RoleId,
          Name = ur.Role.Name,
          Permissions = ur.Role.RolePermissions.Select(rp => rp.Permission.Name).ToArray()
        }).ToArray()
      };
    }

    throw new UnauthorizedException("Wrong Email or Password");
  }
}

public sealed class LoginUserMapper : Mapper<LoginUserRequest, LoginUserResponse, User>
{
  public override User ToEntity(LoginUserRequest r)
    => new(r.Email, r.Password, "", "", false);

  public override LoginUserResponse FromEntity(User e)
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
        Name = ur.Role.Name,
        Permissions = ur.Role.RolePermissions.Select(rp => rp.Permission.Name).ToArray()
      }).ToArray()
    };
}

public sealed class LoginUserValidator
  : AbstractValidator<LoginUserRequest>
{
  public LoginUserValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty()
      .EmailAddress();

    RuleFor(x => x.Password)
      .NotEmpty()
      .MinimumLength(8);
  }
}