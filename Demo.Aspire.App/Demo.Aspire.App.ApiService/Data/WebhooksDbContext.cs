using Demo.Aspire.App.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Aspire.App.ApiService.Data;
internal sealed class WebhooksDbContext(DbContextOptions<WebhooksDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }
    public DbSet<WebhookDeliveryAttempt> WebhookDeliveryAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("orders");
            builder.HasKey(obj => obj.Guid);
        });

        modelBuilder.Entity<WebhookSubscription>(builder =>
        {
            builder.ToTable("subscriptions", "webhooks");
            builder.HasKey(obj => obj.Id);
        });

        modelBuilder.Entity<WebhookDeliveryAttempt>(builder =>
        {
            builder.ToTable("delivery_attempts", "webhooks");
            builder.HasKey(obj => obj.Id);
            builder.HasOne<WebhookSubscription>()
            .WithMany()
            .HasForeignKey(d => d.WebhookSubscriptionId);
        });
    }

}
