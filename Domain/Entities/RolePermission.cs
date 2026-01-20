using Domain.Common;

namespace Domain.Entities;

public class RolePermission : BaseEntity
{
    public string RoleId { get; private set; } = default!;

    public string PermissionId { get; private set; } = default!;

    public Role Role { get; set; } = null!;

    public Permission Permission { get; set; } = null!;

    private RolePermission()
    {
    }

    public RolePermission(string roleId, string permissionId, Role role, Permission permission)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        Role = role;
        Permission = permission;
        CreatedBy = roleId;
        MarkCreated();
    }

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}