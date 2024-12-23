using Microsoft.EntityFrameworkCore;
using Notifier.Web.Common.Models;

namespace Notifier.Web.Features.Email;

public static class EmailApplicationExtensions
{
    public static void AddEmailFeature(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<EmailService>();

        builder.Services.AddDbContext<EmailDbContext>(options =>
        {
            var mongoOptions = builder.Configuration.Get<AppSettings>()!.MongoDbConfigurations;
            options.UseMongoDB(mongoOptions.Host, mongoOptions.DatabaseName);
        });

        builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.SECTION_NAME));

        var emailOptions = builder.Configuration.GetSection(EmailOptions.SECTION_NAME).Get<EmailOptions>()!;
        builder.Services.AddFluentEmail(emailOptions.SenderEmail)
                        .AddSmtpSender(emailOptions.Host, emailOptions.Port, emailOptions.UserName, emailOptions.Password);
    }
}