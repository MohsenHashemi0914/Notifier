namespace Notifier.Web.Common;

public sealed class ProviderNotFoundException(string providerName) : Exception(string.Format(ERROR_TEMPLATE, providerName))
{
    private const string ERROR_TEMPLATE = "Provider `{0}` could not found.";
}