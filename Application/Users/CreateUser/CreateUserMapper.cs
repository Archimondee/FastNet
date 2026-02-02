using Domain.Entities;
using FastEndpoints;

namespace Application.Users.CreateUser;

public sealed class CreateUserMapper
    : Mapper<CreateUserRequest, CreateUserResponse, User>
{
    public override User ToEntity(CreateUserRequest r)
        => new(r.Email, r.Password, r.FirstName, r.LastName, r.IsActive);

    public override CreateUserResponse FromEntity(User e)
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