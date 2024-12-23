using Notifier.Web.Common.Models;
using System.Reflection;

namespace Notifier.Web.Common.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureBroker(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(configure =>
        {
            var brokerConfig = builder.Configuration.GetSection(BrokerOptions.SectionName)
                                                    .Get<BrokerOptions>();

            ArgumentNullException.ThrowIfNull(brokerConfig, nameof(BrokerOptions));

            configure.AddConsumers(Assembly.GetExecutingAssembly());

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(brokerConfig.Host, hostConfigure =>
                {
                    hostConfigure.Username(brokerConfig.UserName);
                    hostConfigure.Password(brokerConfig.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }

    public static void ConfigureAppSettings(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<AppSettings>(builder.Configuration);
    }
}