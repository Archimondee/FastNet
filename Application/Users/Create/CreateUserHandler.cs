using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Exceptions;

namespace Application.Users.Create;

public sealed class CreateUserHandler
{
    private readonly IAppDbContext _db;

    public CreateUserHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        CreateUserCommand command,
        CancellationToken ct)
    {
        var exists = await _db.Users
            .AnyAsync(x => x.Email == command.Email, ct);

        if (exists)
        {
            throw new ConflictException(
                "Email already exists");
        }

        var user = new User(
            command.Email,
            command.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return user.Id;
    }
}