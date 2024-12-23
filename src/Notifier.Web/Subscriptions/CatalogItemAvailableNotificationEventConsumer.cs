using Notifier.Web.Features.Sms;

namespace Notifier.Web.Subscriptions;

public class CatalogItemAvailableNotificationEventConsumer(SmsService smsService) : IConsumer<CatalogItemAvailableNotificationEvent>
{
    public async Task Consume(ConsumeContext<CatalogItemAvailableNotificationEvent> context)
    {
        foreach (var notifyItem in context.Message.Items)
        {
            if (notifyItem.Channel is NotificationChannel.Sms)
            {
                
            }
        }
    }
}