namespace Application.Users.CreateUser;

public sealed class CreateUserCommand
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}