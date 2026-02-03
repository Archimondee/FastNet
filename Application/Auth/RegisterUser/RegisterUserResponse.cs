using Shared.Commons.Response;

namespace Application.Auth.RegisterUser;

public sealed class RegisterUserResponse : BaseResponse
{
  public new Guid Id { get; set; }

  public string Email { get; set; } = default!;

  public string FirstName { get; set; } = default!;

  public string LastName { get; set; } = default!;

  public bool IsActive { get; set; }

  public RoleResponse[] Roles { get; set; } = Array.Empty<RoleResponse>();
}

public sealed class RoleResponse
{
  public Guid Id { get; set; }

  public string Name { get; set; } = default!;
}
