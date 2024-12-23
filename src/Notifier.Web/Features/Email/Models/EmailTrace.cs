using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace Notifier.Web.Features.Email.Models;

[Collection("email_traces")]
public sealed class EmailTrace
{
    public ObjectId Id { get; set; }

    public string SenderEmail { get; set; }

    public string TrackId { get; set; }

    public string Subject { get; set; }

    public string Email { get; set; }

    public string Body { get; set; }

    public EmailTraceStatus Status { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public static EmailTrace Create(string senderEmail, string subject, string trackId, string email, string body)
    {
        return new()
        {
            Body = body,
            Email = email,
            Subject = subject,
            TrackId = trackId,
            SenderEmail = senderEmail,
            Status = EmailTraceStatus.Notified
        };
    }
}