using Shared.Commons.Response;

namespace Application.Users.UpdateUser;

public sealed class UpdateUserResponse : BaseResponse
{
  public new Guid Id { get; set; }

  public string Email { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public string Role { get; set; }

  public bool IsActive { get; set; }

  public RoleResponse[] Roles { get; set; } = Array.Empty<RoleResponse>();
}

public sealed class RoleResponse
{
  public Guid Id { get; set; }

  public string Name { get; set; }
}