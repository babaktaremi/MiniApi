using Microsoft.EntityFrameworkCore;
using MiniApi.MigrationService;
using MiniApi.Shared.Database;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<MiniApiDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("miniDb"), o =>
    {
        o.MigrationsAssembly(typeof(MiniApiDbContext).Assembly);
        optionsBuilder.UseLowerCaseNamingConvention();
    });
});

var host = builder.Build();
host.Run();
