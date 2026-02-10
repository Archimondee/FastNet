namespace Application.Users.UpdateUser;

public class UpdateUserRequest
{
  public string? Email { get; set; }

  public string? FirstName { get; set; }

  public string? LastName { get; set; }

  public string? Password { get; set; }
}