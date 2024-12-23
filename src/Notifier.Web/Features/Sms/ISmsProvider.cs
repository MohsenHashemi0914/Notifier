using Notifier.Web.Features.Sms.Models;
using Notifier.Web.Features.Sms.Providers;

namespace Notifier.Web.Features.Sms;

public interface ISmsProvider
{
    string Name { get; }

    Task<SmsTraceStatus> InquiryAsync(string inquiryId, CancellationToken cancellationToken);

    Task<SmsProviderServiceResult> SendAsync(string mobile, string message, CancellationToken cancellationToken);
}