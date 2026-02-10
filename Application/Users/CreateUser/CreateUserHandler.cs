using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Exceptions;

namespace Application.Users.CreateUser;

public sealed class CreateUserHandler
{
    private readonly IAppDbContext _db;
    private readonly ISecurityHasher _hasher;

    public CreateUserHandler(IAppDbContext db, ISecurityHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    public async Task<User> Handle(
        User user,
        string role,
        CancellationToken ct)
    {
        var exists = await _db.Users
            .AnyAsync(x => x.Email == user.Email, ct);

        if (exists)
            throw new ConflictException("Email already exists");

        user.Password = _hasher.HashPassword(user.Password);

        _db.Users.Add(user);

        var roles = await _db.Roles
                        .FirstOrDefaultAsync(x => x.Name == role, ct)
                    ?? throw new InvalidOperationException("Default role not found");

        _db.UserRoles.Add(new UserRole(user.Id, roles.Id, DateTime.UtcNow, user));
        user.MarkCreated(createdBy: user.Email);

        await _db.SaveChangesAsync(ct);

        var response = await _db.Users.Where(u => u.Id == user.Id)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(ct);

        if (response != null) return response;

        throw new NotFoundException("User not found");
    }
}