using Microsoft.Extensions.Options;
using Notifier.Web.Features.Sms.Models;
using PayamakCore.Dto;
using PayamakCore.Interface;

namespace Notifier.Web.Features.Sms.Providers;

public sealed class FaraPayamakSmsProvider(
    IOptions<SmsOptions> smsOptions,
    IPayamakServices payamakServices,
    ILogger<KavenegarSmsProvider> logger) : ISmsProvider
{
    private readonly SmsOptions.FaraPayamakOptions _options = smsOptions.Value.Providers.FaraPayamak;

    public string Name => "FaraPayamak";

    public async Task<SmsTraceStatus> InquiryAsync(string inquiryId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await payamakServices.GetMessageStatus(new DeliverRequestDto
            {
                RecId = long.Parse(inquiryId),
                username = _options.UserName,
                password = _options.Password,
            });

            // Example
            if (result.Value is "0" or "2")
            {
                return SmsTraceStatus.Inquiry;
            }

            // Better use enum instead of magic values
            if (result.Value is "300")
            {
                return SmsTraceStatus.Success;
            }

            // Handle statuses

            throw new InquirySmsProviderException(Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(FaraPayamakSmsProvider)}.{nameof(InquiryAsync)} has exception.");
            return SmsTraceStatus.Inquiry;
        }
    }

    public async Task<SmsProviderServiceResult> SendAsync(string mobile, string message, CancellationToken cancellationToken)
    {
        try
        {
            var result = await payamakServices.SendSms(new MessageDto
            {
                From = _options.SenderNumber,
                To = mobile,
                Text = message,
                username = _options.UserName,
                password = _options.Password,
                IsFlash = false
            });

            if (result.RetStatus is 1)
            {
                return new(true, result.Value);
            }

            throw new SendSmsProviderException(Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(FaraPayamakSmsProvider)}.{nameof(SendAsync)} has exception.");
            return new(false, string.Empty);
        }
    }
}