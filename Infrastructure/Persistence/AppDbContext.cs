using System.Reflection;
using Application.Interface;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class AppDbContext
    : DbContext, IAppDbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entity.ClrType))
            {
                modelBuilder.Entity(entity.ClrType)
                    .Property<DateTime>("CreatedAt")
                    .IsRequired();
                modelBuilder.Entity(entity.ClrType)
                    .Property<DateTime?>("UpdatedAt");
                modelBuilder.Entity(entity.ClrType)
                    .Property<DateTime?>("DeletedAt");
                modelBuilder.Entity(entity.ClrType)
                    .Property<string?>("CreatedBy");
                modelBuilder.Entity(entity.ClrType)
                    .Property<string?>("DeletedBy");
                modelBuilder.Entity(entity.ClrType)
                    .Property<string?>("UpdatedBy");

                var method = typeof(AppDbContext)
                    .GetMethod(
                        nameof(SetSoftDeleteFilter),
                        BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(entity.ClrType);

                method.Invoke(null, new object[] { modelBuilder });
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInformation()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkUpdated();
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.MarkDeleted();
            }
        }
    }

    private static void SetSoftDeleteFilter<TEntity>(
        ModelBuilder builder)
        where TEntity : BaseEntity
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(e => e.DeletedAt == null);
    }
}