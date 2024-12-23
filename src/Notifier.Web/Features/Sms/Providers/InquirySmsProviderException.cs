namespace Notifier.Web.Features.Sms.Providers;

public sealed class InquirySmsProviderException(string providerName) : Exception(string.Format(ERROR_TEMPLATE, providerName))
{
    private const string ERROR_TEMPLATE = "Provider `{0}` can't inquiry sms.";
}

