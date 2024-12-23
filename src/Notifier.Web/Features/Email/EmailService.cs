using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notifier.Web.Features.Email.Models;

namespace Notifier.Web.Features.Email;

public sealed class EmailService(
    EmailDbContext context,
    IFluentEmail fluentEmail,
    ILogger<EmailService> logger,
    IOptions<EmailOptions> options)
{
    private readonly EmailOptions _options = options.Value;

    public async Task<bool> SendAsync(string email, string subject, string body, CancellationToken cancellationToken)
    {

        var trackId = Guid.NewGuid().ToString();
        var trackingUrl = Path.Combine(_options.TrackingUrl, trackId);
        body += $"<img src='{trackingUrl}' width='1' height='1' />";

        var mail = fluentEmail.To(email)
                              .Subject(subject)
                              .Body(body, isHtml: true);

        try
        {
            var sendResult = await mail.SendAsync(cancellationToken);
            if (sendResult is { Successful: false })
            {
                throw new Exception(message: string.Join(", ", sendResult.ErrorMessages));
            }

            var emailTrace = EmailTrace.Create(_options.SenderEmail, subject, trackId, email, body);
            await context.AddAsync(emailTrace, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Can't send email.");
            return false;
        }
    }

    public async Task OpenEmailAsync(string trackId, CancellationToken cancellationToken)
    {
        var emailTrace = await context.EmailTraces
            .FirstOrDefaultAsync(x => x.TrackId == trackId && x.Status != EmailTraceStatus.Openned, cancellationToken);

        if (emailTrace is null)
        {
            throw new Exception($"Email trace with trackId {trackId} could not found.");
        }

        emailTrace.Status = EmailTraceStatus.Openned;
        _ = await context.SaveChangesAsync(cancellationToken);
    }
}