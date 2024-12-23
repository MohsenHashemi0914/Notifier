namespace Notifier.Web.Common.Models.Enums;

[Flags]
public enum NotificationChannel
{
    Sms = 1,
    Email = 2,
    MsTeams = 3,
    Telegram = 4,
    Slack = 5
}