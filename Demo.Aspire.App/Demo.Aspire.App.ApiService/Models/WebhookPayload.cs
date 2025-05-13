namespace Demo.Aspire.App.ApiService.Models
{
    public class WebhookPayload<T>
    {
        public Guid Id { get; set; }
        public string EventType { get; set; } = string.Empty;
        public Guid SubscriptionId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public required T Data { get; set; }
    }
}
