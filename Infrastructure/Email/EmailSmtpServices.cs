using Application.Interface;
using Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Email;

public class EmailSmtpServices : IEmailSender
{
  private readonly SmtpOptions _smtp;
  private readonly IEmailTemplateRenderer _renderer;
  private readonly RateLimiter _rateLimiter;
  private readonly ILogger<EmailSmtpServices> _logger;

  private const int MaxRetry = 3;

  public EmailSmtpServices(
    IOptions<SmtpOptions> smtp,
    IEmailTemplateRenderer renderer,
    ILogger<EmailSmtpServices> logger,
    RateLimiter rateLimiter)
  {
    _smtp = smtp.Value;
    _renderer = renderer;
    _logger = logger;
    _rateLimiter = rateLimiter;
  }

  public async Task SendEmailAsync(EmailOutbox email, CancellationToken ct = default)
  {
    if (string.IsNullOrWhiteSpace(email.To))
      throw new ArgumentException("Email recipient is required");

    await _rateLimiter.WaitAsync(ct);

    var message = await BuildMessageAsync(email, ct);

    for (int attempt = 1; attempt <= MaxRetry; attempt++)
    {
      try
      {
        using var client = new SmtpClient();
        client.Timeout = 15000;

        await client.ConnectAsync(
          _smtp.Host,
          _smtp.Port,
          SecureSocketOptions.StartTls,
          ct);

        await client.AuthenticateAsync(
          _smtp.Username,
          _smtp.Password,
          ct);

        await client.SendAsync(message, ct);
        await client.DisconnectAsync(true, ct);

        email.ProviderMessageId = message.MessageId;

        _logger.LogInformation(
          "Email sent to {Recipient} (Attempt {Attempt})",
          email.To,
          attempt);

        return;
      }
      catch (Exception ex) when (attempt < MaxRetry)
      {
        var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));

        _logger.LogWarning(ex,
          "Email send failed. Retrying in {Delay}s (Attempt {Attempt})",
          delay.TotalSeconds,
          attempt);

        await Task.Delay(delay, ct);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex,
          "Email sending failed permanently for {Recipient}",
          email.To);

        throw;
      }
    }
  }

  private async Task<MimeMessage> BuildMessageAsync(
    EmailOutbox email,
    CancellationToken ct)
  {
    var message = new MimeMessage();

    message.From.Add(
      new MailboxAddress(
        _smtp.SenderName,
        _smtp.SenderEmail));

    message.To.Add(MailboxAddress.Parse(email.To!));
    message.Subject = email.Template;

    if (!string.IsNullOrEmpty(email.PayloadJson))
    {
      var html = await _renderer.RenderAsync(
        email.Template,
        email.PayloadJson);

      message.Body = new BodyBuilder
      {
        HtmlBody = html
      }.ToMessageBody();
    }

    return message;
  }
}

public class SmtpOptions
{
  public string Host { get; set; } = default!;

  public int Port { get; set; }

  public string Username { get; set; } = default!;

  public string Password { get; set; } = default!;

  public string SenderName { get; set; } = default!;

  public string SenderEmail { get; set; } = default!;

  public bool UseStartTls { get; set; }
}