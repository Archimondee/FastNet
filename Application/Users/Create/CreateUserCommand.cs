namespace Application.Users.Create;

public sealed class CreateUserCommand
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}