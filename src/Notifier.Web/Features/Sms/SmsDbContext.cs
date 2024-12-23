using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using Notifier.Web.Features.Sms.Models;

namespace Notifier.Web.Features.Sms;

public sealed class SmsDbContext(DbContextOptions<SmsDbContext> options) : DbContext(options)
{
    public DbSet<SmsTrace> SmsTraces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<SmsTrace>().ToCollection("sms_traces");
    }
}