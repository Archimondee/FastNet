using Application.Interface;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
    }

    public async Task BeginAsync(CancellationToken ct)
        => await _db.Database.BeginTransactionAsync(ct);

    public async Task CommitAsync(CancellationToken ct)
        => await _db.Database.CommitTransactionAsync(ct);

    public async Task RollbackAsync(CancellationToken ct)
        => await _db.Database.RollbackTransactionAsync(ct);
}