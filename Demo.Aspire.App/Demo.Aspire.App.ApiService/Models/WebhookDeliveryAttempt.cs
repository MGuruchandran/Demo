namespace Demo.Aspire.App.ApiService.Models;

public class WebhookDeliveryAttempt
{
    public Guid Id { get; set; }
    public Guid WebhookSubscriptionId { get; set; }
    public string Payload { get; set; }
    public int? ResponseStatusCode { get; set; }
    public bool Success { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
}

