using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interface;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
  private readonly JwtSettings _jwtSettings;

  public JwtTokenGenerator(JwtSettings jwtSettings)
  {
    _jwtSettings = jwtSettings;
  }

  public string GenerateToken(User user)
  {
    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(ClaimTypes.Role, user.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault() ?? string.Empty),
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: _jwtSettings.Issuer,
      audience: _jwtSettings.Audience,
      claims: claims,
      expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
      signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}

public sealed class JwtSettings
{
  public string Secret { get; init; } = default!;

  public string Issuer { get; init; } = default!;

  public string Audience { get; init; } = default!;

  public int ExpiryMinutes { get; init; }
}
