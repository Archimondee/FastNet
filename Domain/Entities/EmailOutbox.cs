namespace Domain.Entities;

public class EmailOutbox
{
  public Guid Id { get; set; }

  public string? To { get; set; }

  public string Template { get; set; } = default!;

  public string? PayloadJson { get; set; }

  public EmailStatus Status { get; set; } = EmailStatus.Pending;

  public int RetryCount { get; set; }

  public DateTime? NextRetryAt { get; set; }

  public DateTime ScheduledAt { get; set; }

  public string? ProviderMessageId { get; set; }

  public string? ErrorMessage { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime? SentAt { get; set; }

  public string? CorrelationId { get; set; }
}

public enum EmailStatus
{
  Pending,
  Processing,
  Sent,
  Failed
}