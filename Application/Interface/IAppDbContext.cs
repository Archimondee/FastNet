using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interface;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}