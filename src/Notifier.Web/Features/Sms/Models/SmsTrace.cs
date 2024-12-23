using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace Notifier.Web.Features.Sms.Models;

[Collection("sms_traces")]
public sealed class SmsTrace
{
    public ObjectId Id { get; set; }

    public string ProviderName { get; set; }

    public string InquiryId { get; set; }

    public string Mobile { get; set; }

    public string Message { get; set; }

    public SmsTraceStatus Status { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public static SmsTrace Create(string providerName, string inquiryId, string mobile, string message)
    {
        return new()
        {
            Mobile = mobile,
            Message = message,
            InquiryId = inquiryId,
            ProviderName = providerName,
            Status = SmsTraceStatus.Inquiry
        };
    }
}