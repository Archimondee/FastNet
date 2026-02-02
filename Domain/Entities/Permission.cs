using Domain.Common;

namespace Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    private Permission()
    {
    }

    public Permission(string name, string description, User user)
    {
        Name = name;
        Description = description;
        CreatedBy = user.Email;
        MarkCreated();
    }

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}