using Microsoft.EntityFrameworkCore;
using Notifier.Web.Common.Models;
using Notifier.Web.Features.Sms.Providers;
using PayamakCore;

namespace Notifier.Web.Features.Sms;

public static class SmsApplicationExtensions
{
    public static void AddSmsFeature(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<SmsService>();
        builder.Services.AddHostedService<SmsInquiryBackgroundService>();
        builder.Services.AddScoped<ISmsProvider, KavenegarSmsProvider>();
        builder.Services.AddScoped<ISmsProvider, FaraPayamakSmsProvider>();

        builder.Services.AddDbContext<SmsDbContext>(options =>
        {
            var mongoOptions = builder.Configuration.Get<AppSettings>()!.MongoDbConfigurations;
            options.UseMongoDB(mongoOptions.Host, mongoOptions.DatabaseName);
        });

        builder.Services.Configure<SmsOptions>(builder.Configuration.GetSection(SmsOptions.SECTION_NAME));
        builder.Services.AddPayamakService();
    }
}