using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using Application.Interface;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class AppDbContext
    : DbContext, IAppDbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

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
        var currentUserEmail = GetCurrentUserEmai();
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkCreated(createdBy: currentUserEmail);
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkUpdated(updatedBy: currentUserEmail);
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.MarkDeleted(deletedBy: currentUserEmail);
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

    private string GetCurrentUserEmai()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
            return "system";

        return
            user.FindFirstValue(JwtRegisteredClaimNames.Email) ??
            "system";
    }
}