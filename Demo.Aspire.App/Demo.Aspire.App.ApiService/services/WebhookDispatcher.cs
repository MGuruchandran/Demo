using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Demo.Aspire.App.ApiService.Data;
using Demo.Aspire.App.ApiService.Models;

namespace Demo.Aspire.App.ApiService.Services;

internal sealed class WebhookDispatcher(IHttpClientFactory httpClientfactory, WebhooksDbContext dbContext)
{
    public async Task DispatchAsync<T>(string eventType, T data)
    {
        var subscriptions = await dbContext.WebhookSubscriptions.AsNoTracking().Where(m => m.EventType == eventType).ToListAsync();// subscriptionRepository.GetByEventType(eventType);

        foreach (var item in subscriptions)
        {
            using var httpClient = httpClientfactory.CreateClient();
            var payload = new WebhookPayload<T>
            {
                Id = Guid.NewGuid(),
                EventType = item.EventType,
                SubscriptionId = item.Id,
                TimeStamp = DateTimeOffset.UtcNow,
                Data = data
            };
            var jsonPayload = JsonSerializer.Serialize(payload);
            HttpResponseMessage responseMessage;
            try
            {
                responseMessage = await httpClient.PostAsJsonAsync(item.webHookUrl, payload);
                var attempt = new WebhookDeliveryAttempt
                {
                    Id = Guid.NewGuid(),
                    Payload = jsonPayload,
                    ResponseStatusCode = (int)responseMessage.StatusCode,
                    Success = responseMessage.IsSuccessStatusCode,
                    TimeStamp = DateTimeOffset.UtcNow,
                    WebhookSubscriptionId = item.Id

                };
                dbContext.WebhookDeliveryAttempts.Add(attempt);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var attempt = new WebhookDeliveryAttempt
                {
                    Id = Guid.NewGuid(),
                    Payload = jsonPayload,
                    ResponseStatusCode = null,
                    Success = false,
                    TimeStamp = DateTimeOffset.UtcNow,
                    WebhookSubscriptionId = item.Id

                };
                dbContext.WebhookDeliveryAttempts.Add(attempt);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}