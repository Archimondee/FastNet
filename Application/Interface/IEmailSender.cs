using Domain.Entities;

namespace Application.Interface;

public interface IEmailSender
{
  Task SendEmailAsync(EmailOutbox email, CancellationToken ct = default);
}