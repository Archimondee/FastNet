using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Infrastructure.Scheduling.Jobs;

public class EmailDispatcherJob : IJob
{
  private readonly IAppDbContext _db;

  private readonly IEmailSender _sender;

  public EmailDispatcherJob(IAppDbContext db, IEmailSender sender)
  {
    _db = db;
    _sender = sender;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    var now = DateTime.UtcNow;

    var emails = await _db.EmailOutboxes
      .Where(x =>
        x.Status == EmailStatus.Pending &&
        x.ScheduledAt <= now &&
        (x.NextRetryAt == null || x.NextRetryAt <= now))
      .OrderBy(x => x.CreatedAt)
      .Take(20)
      .ToListAsync();

    foreach (var email in emails)
    {
      try
      {
        email.Status = EmailStatus.Processing;

        await _sender.SendEmailAsync(email);

        email.Status = EmailStatus.Sent;
        email.SentAt = DateTime.UtcNow;
      }
      catch (Exception ex)
      {
        email.RetryCount++;
        email.ErrorMessage = ex.Message;

        if (email.RetryCount >= 5)
        {
          email.Status = EmailStatus.Failed;
        }
        else
        {
          email.Status = EmailStatus.Pending;
          email.NextRetryAt =
            DateTime.UtcNow.AddMinutes(
              Math.Pow(2, email.RetryCount));
        }
      }
    }

    await _db.SaveChangesAsync(context.CancellationToken);
  }
}