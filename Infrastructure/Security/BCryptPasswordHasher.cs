using Application.Interface;

namespace Infrastructure.Security;

public class BCryptPasswordHasher : ISecurityHasher
{
  public string HashPassword(string password)
  {
    return BCrypt.Net.BCrypt.HashPassword(password, 12);
  }

  public bool VerifyPassword(string hashedPassword, string password)
  {
    return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
  }
}