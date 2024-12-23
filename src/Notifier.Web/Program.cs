using Microsoft.AspNetCore.Mvc;
using Notifier.Web.Common.Extensions;
using Notifier.Web.Features.Email;
using Notifier.Web.Features.Sms;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppSettings();
builder.ConfigureBroker();
builder.AddSmsFeature();
builder.AddEmailFeature();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/sms", async (SmsService smsService, CancellationToken cancellationToken) =>
{
    await smsService.SendAsync("09xxxxxxxxx", "Just for test", cancellationToken);
});

app.MapPost("/email", async (string email, string subject, string body,
    EmailService emailService, CancellationToken cancellationToken) =>
{
    await emailService.SendAsync(email, subject, body, cancellationToken);
});

app.MapPost("/email/tracking/{track_Id}", async ([FromRoute(Name = "track_Id")] string trackId,
    EmailService emailService, CancellationToken cancellationToken) =>
{
    await emailService.OpenEmailAsync(trackId, cancellationToken);
});

app.Run();