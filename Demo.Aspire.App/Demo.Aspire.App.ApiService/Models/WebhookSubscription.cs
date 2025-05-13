namespace Demo.Aspire.App.ApiService.Models;

public sealed record WebhookSubscription(Guid Id, string EventType, string webHookUrl,DateTimeOffset CreatedOnUtc);
public sealed record CreateWebHookRequest(string EventType, string webHookUrl);

