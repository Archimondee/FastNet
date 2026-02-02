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
        CancellationToken ct)
    {
        var exists = await _db.Users
            .AnyAsync(x => x.Email == user.Email, ct);

        if (exists)
            throw new ConflictException("Email already exists");

        user.Password = _hasher.HashPassword(user.Password);

        _db.Users.Add(user);

        var role = await _db.Roles
                       .FirstOrDefaultAsync(x => x.Name == "User", ct)
                   ?? throw new InvalidOperationException("Default role not found");

        _db.UserRoles.Add(new UserRole(user.Id, role.Id, DateTime.UtcNow, user));

        await _db.SaveChangesAsync(ct);

        var response = await _db.Users.Where(u => u.Id == user.Id)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(ct);

        if (response != null) return response;

        throw new NotFoundException("User not found");
    }
}