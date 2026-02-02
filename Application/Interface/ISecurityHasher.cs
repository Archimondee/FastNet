namespace Application.Interface;

public interface ISecurityHasher
{
  string HashPassword(string password);

  bool VerifyPassword(string hashedPassword, string password);
}