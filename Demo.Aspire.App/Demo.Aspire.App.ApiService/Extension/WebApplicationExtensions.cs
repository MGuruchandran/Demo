using Microsoft.EntityFrameworkCore;
using Demo.Aspire.App.ApiService.Data;

namespace Demo.Aspire.App.ApiService.Extension;
public static class WebApplicationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WebhooksDbContext>();
        await db.Database.MigrateAsync();
    }
}
