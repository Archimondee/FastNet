namespace Domain.Dtos;

public sealed class RolePermissionSeedDto
{
  public string Role { get; set; } = default!;

  public List<string> Permissions { get; set; } = [];
}