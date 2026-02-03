namespace Application.Auth.RegisterUser;

public sealed class RegisterUserRequest
{
  public string Email { get; set; } = default!;

  public string Password { get; set; } = default!;

  public string FirstName { get; set; } = default!;

  public string LastName { get; set; } = default!;

  public bool? IsActive { get; set; } = true;
}