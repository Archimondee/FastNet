namespace Application.Users;

public sealed record CreateUserCommand(string Email, string Password);