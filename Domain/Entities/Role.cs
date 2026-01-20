using Domain.Common;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    private Role()
    {
    }

    public Role(string name, string description)
    {
        Name = name;
        Description = description;
        CreatedBy = name;
        MarkCreated();
    }

    public ICollection<UserRole> UserRoles { get; set; } = [];

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}