using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class EmailOutboxConfiguration : IEntityTypeConfiguration<EmailOutbox>
{
  public void Configure(EntityTypeBuilder<EmailOutbox> builder)
  {
    builder.ToTable("email_outboxes");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.To).IsRequired();

    builder.Property(x => x.Template).IsRequired();

    builder.Property(x => x.PayloadJson).IsRequired();

    builder.Property(x => x.Status).HasDefaultValue(EmailStatus.Pending);

    builder.Property(x => x.RetryCount).HasDefaultValue(0);

    builder.Property(x => x.NextRetryAt).HasDefaultValue(null);

    builder.Property(x => x.ProviderMessageId).HasDefaultValue(null);

    builder.Property(x => x.ScheduledAt).IsRequired();

    builder.Property(x => x.ErrorMessage).HasDefaultValue(null);

    builder.Property(x => x.CreatedAt).IsRequired();

    builder.Property(x => x.SentAt).HasDefaultValue(null);

    builder.Property(x => x.CorrelationId).HasDefaultValue(null);

    builder.HasIndex(x => x.Id);

    builder.HasIndex(x => x.To);

    builder.HasIndex(x => x.Status);
  }
}