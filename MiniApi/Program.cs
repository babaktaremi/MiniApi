using MiniApi.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.AddDbContext()
    .AddDapper()
    .AddMediator()
    .MapMinimalEndpoints(typeof(Program).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMinimalEndpoints();

await app.MigrateDatabaseAsync();

await app.RunAsync();
