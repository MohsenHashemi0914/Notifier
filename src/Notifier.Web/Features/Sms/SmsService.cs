using Notifier.Web.Common;
using Notifier.Web.Features.Sms.Models;

namespace Notifier.Web.Features.Sms;

public sealed class SmsService(IEnumerable<ISmsProvider> providers, SmsDbContext dbContext)
{
    public async Task<bool> SendAsync(string mobile, string message, CancellationToken cancellationToken)
    {
        foreach (var provider in providers)
        {
            var result = await provider.SendAsync(mobile, message, cancellationToken);
            if (!result.IsSuccessful)
            {
                continue;
            }

            var trace = SmsTrace.Create(provider.Name, result.InquiryId, mobile, message);
            await dbContext.AddAsync(trace, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }

    public async Task<SmsTraceStatus> InquiryAsync(SmsTrace smsTrace, CancellationToken cancellationToken)
    {
        var provider = providers.FirstOrDefault(x => x.Name == smsTrace.ProviderName);
        if (provider is null)
        {
            throw new ProviderNotFoundException(smsTrace.ProviderName);
        }

        return await provider.InquiryAsync(smsTrace.InquiryId, cancellationToken);
    }
}