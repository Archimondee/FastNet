using Domain.Common;

namespace Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; private set; } = default!;

    public string Password { get; private set; } = default!;


    private User()
    {
    }

    public User(string email, string passwordHash)
    {
        Email = email;
        Password = passwordHash;
        CreatedBy = email;
        MarkCreated();
    }
}