using Microsoft.EntityFrameworkCore;
using MiniApi.Shared.Database;

namespace MiniApi.MigrationService;

public class Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
   

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogWarning("Applying Migrations");
        using var scope= serviceScopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<MiniApiDbContext>();

        var strategy = db.Database.CreateExecutionStrategy();
        
       await strategy.ExecuteAsync(async () =>
        {
            await db.Database.MigrateAsync(stoppingToken);
        });
       
       logger.LogWarning("Migrations Applied");
    }
}
