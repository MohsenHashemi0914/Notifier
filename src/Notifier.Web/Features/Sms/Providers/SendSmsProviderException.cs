namespace Notifier.Web.Features.Sms.Providers;

public sealed class SendSmsProviderException(string providerName) : Exception(string.Format(ERROR_TEMPLATE, providerName))
{
    private const string ERROR_TEMPLATE = "Provider `{0}` can't send sms.";
}