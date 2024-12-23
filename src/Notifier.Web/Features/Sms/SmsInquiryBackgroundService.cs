
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notifier.Web.Features.Sms.Models;

namespace Notifier.Web.Features.Sms;

public sealed class SmsInquiryBackgroundService : IHostedService
{
    private readonly SmsDbContext _context;
    private readonly SmsService _smsService;
    private readonly SmsOptions _smsOptions;
    private readonly ILogger<SmsInquiryBackgroundService> _logger;

    public SmsInquiryBackgroundService(
        IOptions<SmsOptions> smsOptions,
        IServiceScopeFactory scopeFactory,
        ILogger<SmsInquiryBackgroundService> logger)
    {
        _logger = logger;
        _smsOptions = smsOptions.Value;

        using var scope = scopeFactory.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<SmsDbContext>();
        _smsService = scope.ServiceProvider.GetRequiredService<SmsService>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sms inquiry job has started...");

        Task.Run(async () =>
        {
            while (true)
            {
                await InquirySms(cancellationToken);
                await Task.Delay(_smsOptions.InquiryPeriodInSeconds * 1000, cancellationToken);
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sms inquiry job has stopped...");
        return Task.CompletedTask;
    }

    private async Task InquirySms(CancellationToken cancellationToken)
    {
        var smsListWithInquiryStatus = await _context.SmsTraces
            .Where(x => x.Status == SmsTraceStatus.Inquiry)
            .ToListAsync(cancellationToken);

        if(smsListWithInquiryStatus is [])
        {
            return;
        }

        _context.AttachRange(smsListWithInquiryStatus);
        foreach (var smsTrace in smsListWithInquiryStatus)
        {
            smsTrace.Status = await _smsService.InquiryAsync(smsTrace, cancellationToken);
            _context.Entry(smsTrace).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}