using Domain.Common;

namespace Domain.Entities;

public class UserRole : BaseEntity
{
    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    public DateTime AssignedAt { get; private set; }

    public User User { get; set; } = null!;

    public Role Role { get; set; } = null!;

    private UserRole()
    {
    }

    public UserRole(Guid userId, Guid roleId, User user, Role role, DateTime assignedAt)
    {
        UserId = userId;
        RoleId = roleId;
        User = user;
        Role = role;
        AssignedAt = assignedAt;
        CreatedBy = user.Email;
        MarkCreated();
    }
}