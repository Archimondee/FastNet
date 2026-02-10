using Application.Interface;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Exceptions;

namespace Application.Users.DeleteUser;

public class DeleteUserHandler
{
  private readonly IAppDbContext _db;

  public DeleteUserHandler(IAppDbContext db)
  {
    _db = db;
  }

  public async Task<object> Handle(string userId, CancellationToken ct)
  {
    if (Guid.TryParse(userId, out var id))
    {
      var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
      if (user is null)
      {
        throw new NotFoundException("User not found");
      }

      _db.Users.Remove(user);
      await _db.SaveChangesAsync(ct);
      return new { };
    }

    throw new NotFoundException("User not found");
  }
}