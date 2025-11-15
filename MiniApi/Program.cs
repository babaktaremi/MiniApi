using MiniApi.Shared.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.AddDbContext()
    .AddDapper()
    .AddMediator()
    .MapMinimalEndpoints(typeof(Program).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGet("healthcheck", () => Results.Ok("Healthy"))
    .WithName("HealthCheck")
    .WithTags("HealthCheck")
    .Produces(200)
    .Produces(500);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMinimalEndpoints();

//await app.MigrateDatabaseAsync();

await app.RunAsync();
