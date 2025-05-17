using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MiniApi.Shared.Database;
using MiniApi.Shared.Endpoints;
using Npgsql;

namespace MiniApi.Shared.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder)
    {

        builder.Services.AddDbContext<MiniApiDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("miniDb"));
            optionsBuilder.UseLowerCaseNamingConvention();
        });
        
        return builder;
    }

    public static WebApplicationBuilder AddDapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDbConnection>(_ =>
            new NpgsqlConnection(builder.Configuration.GetConnectionString("miniDb")));

        return builder;
    }

    public static WebApplicationBuilder AddMediator(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Transient;
            options.Namespace = "MiniApi.MediatorServices";
        });

        return builder;
    }
    
    public static WebApplicationBuilder MapMinimalEndpoints(this WebApplicationBuilder builder,params IEnumerable<Assembly> assemblies)
    {
        var endpoints = assemblies.SelectMany(a => a.GetTypes())
            .Where(c => !c.IsAbstract && c.GetInterfaces().Contains(typeof(IEndpoint)))
            .Select(c => ServiceDescriptor.Transient(typeof(IEndpoint), c));

        builder.Services.TryAddEnumerable(endpoints);

        return builder;
    }
}