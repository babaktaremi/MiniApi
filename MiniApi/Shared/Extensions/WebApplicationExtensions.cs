using Microsoft.EntityFrameworkCore;
using MiniApi.Shared.Database;
using MiniApi.Shared.Endpoints;

namespace MiniApi.Shared.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseMinimalEndpoints(this WebApplication app)
    {
        var registeredEndpoints = app.Services.GetServices<IEndpoint>();

        foreach (var registeredEndpoint in registeredEndpoints)
        {
            registeredEndpoint.MapEndpoint(app);
        }

        return app;
    }

    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<MiniApiDbContext>();

        await db.Database.MigrateAsync();
    }
}