namespace Domain.Entities;

public class UserRole
{
    public string UserId { get; private set; } = default!;

    public string RoleId { get; private set; } = default!;

    public DateTime AssignedAt { get; private set; } = default!;

    public User User { get; set; } = null!;

    public Role Role { get; set; } = null!;

    private UserRole()
    {
    }

    public UserRole(string userId, string roleId, User user, Role role, DateTime assignedAt)
    {
        UserId = userId;
        RoleId = roleId;
        User = user;
        Role = role;
        AssignedAt = assignedAt;
    }
}