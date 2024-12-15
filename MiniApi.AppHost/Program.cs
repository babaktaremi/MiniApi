var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("miniApiDb")
    .WithImageRegistry("docker.arvancloud.ir")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgWeb(resourceBuilder =>
    {
        resourceBuilder.WithImageRegistry("docker.arvancloud.ir");
    })
    .WithPgAdmin(resourceBuilder =>
    {
        resourceBuilder.WithImageRegistry("docker.arvancloud.ir");
    });

builder.AddProject<Projects.MiniApi>("miniApi")
    .WithExternalHttpEndpoints()
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();