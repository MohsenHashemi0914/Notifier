namespace Notifier.Web.Features.Email;

public sealed class EmailOptions
{
    public const string SECTION_NAME = $"{nameof(Features)}:{nameof(Email)}";

    public required int Port { get; set; }

    public required string Host { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string SenderEmail { get; set; }

    public required string TrackingUrl { get; set; }
}