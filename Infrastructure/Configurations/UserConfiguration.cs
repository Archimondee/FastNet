using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Configurations;

public sealed class UserConfiguration
    : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(x => x.Email)
            .IsUnique();
        builder.Property(x => x.Password)
            .IsRequired();
    }
}
