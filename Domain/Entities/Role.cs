using Domain.Common;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;


    private Role()
    {
    }

    public Role(string name, string description, User user)
    {
        Name = name;
        Description = description;
        CreatedBy = user.Email;
        MarkCreated();
    }

    public ICollection<UserRole> UserRoles { get; set; } = [];

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}