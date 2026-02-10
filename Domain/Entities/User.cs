using Domain.Common;

namespace Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public bool IsActive { get; private set; } = true;

    private User()
    {
    }

    public User(string email, string passwordHash, string firstName, string lastName, bool? isActive)
    {
        Email = email;
        Password = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        IsActive = isActive ?? true;
        CreatedBy = email;
        MarkCreated();
    }

    public ICollection<UserRole> UserRoles { get; set; } = [];
}