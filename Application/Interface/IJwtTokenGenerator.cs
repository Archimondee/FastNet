using Domain.Entities;

namespace Application.Interface;

public interface IJwtTokenGenerator
{
  string GenerateToken(User user);
}