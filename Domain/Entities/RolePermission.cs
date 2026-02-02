using Domain.Common;

namespace Domain.Entities;

public class RolePermission : BaseEntity
{
    public Guid RoleId { get; private set; } = default!;

    public Guid PermissionId { get; private set; } = default!;

    public Role Role { get; set; } = null!;

    public Permission Permission { get; set; } = null!;

    private RolePermission()
    {
    }

    public RolePermission(Guid roleId, Guid permissionId, Role role, Permission permission, User user)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        Role = role;
        Permission = permission;
        CreatedBy = user.Email;
        MarkCreated();
    }

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}