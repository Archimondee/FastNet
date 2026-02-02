using Domain.Common;

namespace Domain.Entities;

public class RolePermission : BaseEntity
{
    public Guid RoleId { get; private set; }

    public Guid PermissionId { get; private set; }

    public Role Role { get; set; } = null!;

    public Permission Permission { get; set; } = null!;

    private RolePermission()
    {
    }

    public RolePermission(Guid roleId, Guid permissionId, User? user = null)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        CreatedBy = user?.Email ?? "system@email.com";
        MarkCreated();
    }
}