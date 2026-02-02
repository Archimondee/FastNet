using Domain.Common;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;


    private Role()
    {
    }

    public Role(string name, string description, User? user = null)
    {
        Name = name;
        Description = description;
        CreatedBy = user?.Email ?? "system@email.com";
        MarkCreated();
    }

    public ICollection<UserRole> UserRoles { get; set; } = [];

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}