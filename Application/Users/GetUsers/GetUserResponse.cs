using Shared.Commons.Response;

namespace Application.Users.GetUsers;

public class GetUserResponse : BaseResponse
{
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

  public string[] Permissions { get; set; } = Array.Empty<string>();
}