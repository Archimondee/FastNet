namespace Domain.Dtos;

public sealed class UserSeedDto
{
  public string Email { get; set; } = default!;
  public string Password { get; set; } = default!;
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public bool IsActive { get; set; }
  public string Role { get; set; } = default!;
}