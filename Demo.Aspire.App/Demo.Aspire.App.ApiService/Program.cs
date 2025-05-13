
using Demo.Aspire.App.ApiService.Data;
using Demo.Aspire.App.ApiService.Extension;
using Demo.Aspire.App.ApiService.Models;
using Demo.Aspire.App.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddHttpClient<WebhookDispatcher>();
builder.AddNpgsqlDbContext<WebhooksDbContext>("webhooks");
//builder.Services.AddDbContext<WebhooksDbContext>
//     (options => options.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb")
//      ?? throw new InvalidOperationException("Connection string 'postgresdb' not found.")));

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrationsAsync();
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapDefaultEndpoints();
app.UseHttpsRedirection();

app.MapPost("/orders", async (CreateOrderRequest request, WebhooksDbContext dbContext, WebhookDispatcher webhookDispatcher) =>
{
    var order = new Order(Guid.NewGuid(), request.CustomerName, request.Amount, DateTimeOffset.UtcNow);
    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync();
    await webhookDispatcher.DispatchAsync("order.created", order);
    return Results.Ok(order);
});

app.MapPost("webhooks/subscriptions", async (CreateWebHookRequest request, WebhooksDbContext dbContext) =>
{
    WebhookSubscription subscription = new WebhookSubscription(Guid.NewGuid(), request.EventType, request.webHookUrl, DateTimeOffset.UtcNow);
    dbContext.WebhookSubscriptions.Add(subscription);
    await dbContext.SaveChangesAsync();
    return Results.Ok(subscription);
});

app.MapPost("/orders1", async (CreateOrderRequest createOrderRequest,WebhooksDbContext webhooksDbContext) =>
{
    var order = new Order(Guid.NewGuid(), createOrderRequest.CustomerName, createOrderRequest.Amount, DateTimeOffset.UtcNow);
    webhooksDbContext.Orders.Add(order);
    await webhooksDbContext.SaveChangesAsync();
    //await webhookDispatcher.DispatchAsync("order.created",order);
    return Results.Ok(order);
}).WithTags("Orders");

app.MapGet("/orders", async (WebhooksDbContext dbContext) =>
{
    return Results.Ok(await dbContext.Orders.ToListAsync());
}).WithTags("Orders");
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
