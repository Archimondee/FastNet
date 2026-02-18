using Application.Interface;
using Domain.Entities;
using FastEndpoints;

namespace API.Endpoints.Email;

public class EmailTestEndpoint : Endpoint<EmptyRequest, EmptyResponse>
{
 private readonly IEmailSender _emailSender;
 private readonly IAppDbContext _db;

 public EmailTestEndpoint(IEmailSender emailSender, IAppDbContext db, IUnitOfWork uow)
 {
  _emailSender = emailSender;
  _db = db;
 }

 public override void Configure()
 {
  RoutePrefixOverride("api/v1");
  Get("email/test");
  AllowAnonymous();
  Throttle(hitLimit: 20, durationSeconds: 120);
 }

 public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
 {
  var outbox = new EmailOutbox
  {
   To = "gilangdev97@gmail.com",
   Template = "email-test",
   Subject = "Subject Email",
   PayloadJson = """ 
                 {
                  "Name": "Gilang",
                  "Message": "This is a test email"
                 }
                 """,
  };

  _db.EmailOutboxes.Add(outbox);
  await _db.SaveChangesAsync(ct);

  try
  {
   await _emailSender.SendEmailAsync(outbox, ct);
   await _db.SaveChangesAsync(ct);
  }
  catch
  {
   await _db.SaveChangesAsync(ct);
   throw;
  }

  await Send.OkAsync(ct);
 }
}