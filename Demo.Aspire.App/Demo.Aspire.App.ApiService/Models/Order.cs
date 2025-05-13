namespace Demo.Aspire.App.ApiService.Models;

public sealed record Order(Guid Guid,string CustomerName,decimal Amount, DateTimeOffset CreatedAt);
public sealed record CreateOrderRequest(string CustomerName,decimal Amount);
