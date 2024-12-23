using Kavenegar;
using Kavenegar.Core.Models.Enums;
using Microsoft.Extensions.Options;
using Notifier.Web.Features.Sms.Models;

namespace Notifier.Web.Features.Sms.Providers;

public sealed class KavenegarSmsProvider(IOptions<SmsOptions> smsOptions, ILogger<KavenegarSmsProvider> logger) : ISmsProvider
{
    private readonly SmsOptions.KavenegarOptions _options = smsOptions.Value.Providers.Kavenegar;

    public string Name => "Kavenegar";

    private static readonly List<int> _successStatuses = [1, 2, 4, 5]; // For example

    public async Task<SmsTraceStatus> InquiryAsync(string inquiryId, CancellationToken cancellationToken)
    {
        try
        {
            var api = new KavenegarApi(_options.ApiKey);
            var result = await api.Status(inquiryId);

            // Example
            if (result.Status is MessageStatus.Delivered)
            {
                return SmsTraceStatus.Success;
            }

            // Handle statuses

            throw new InquirySmsProviderException(Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(KavenegarSmsProvider)}.{nameof(InquiryAsync)} has exception.");
            return SmsTraceStatus.Inquiry;
        }
    }

    public async Task<SmsProviderServiceResult> SendAsync(string mobile, string message, CancellationToken cancellationToken)
    {
        try
        {
            var api = new KavenegarApi(_options.ApiKey);
            var result = await api.Send(_options.SenderNumber, mobile, message);

            if (_successStatuses.Exists(x => x == result.Status))
            {
                return new(true, result.Messageid.ToString());
            }

            throw new SendSmsProviderException(Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(KavenegarSmsProvider)}.{nameof(SendAsync)} has exception.");
            return new(false, string.Empty);
        }
    }
}