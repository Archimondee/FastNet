using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Exceptions;

namespace Application.Users.CreateUser;

public sealed class CreateUserHandler
{
    private readonly IAppDbContext _db;

    public CreateUserHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<User> Handle(
        User user,
        CancellationToken ct)
    {
        var exists = await _db.Users
            .AnyAsync(x => x.Email == user.Email, ct);

        if (exists)
            throw new ConflictException("Email already exists");

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return user;
    }
}