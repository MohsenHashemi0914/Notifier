namespace Notifier.Web.Features.Sms;

public sealed class SmsOptions
{
    public const string SECTION_NAME = $"{nameof(Features)}:{nameof(Sms)}";

    public required byte InquiryPeriodInSeconds { get; set; }

    public required SmsProviderOptions Providers { get; set; }

    public sealed class SmsProviderOptions
    {
        public required KavenegarOptions Kavenegar { get; set; }

        public required FaraPayamakOptions FaraPayamak { get; set; }
    }

    public sealed class FaraPayamakOptions
    {
        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required string SenderNumber { get; set; }
    }

    public sealed class KavenegarOptions
    {
        public required string ApiKey { get; set; }

        public required string SenderNumber { get; set; }
    }
}
