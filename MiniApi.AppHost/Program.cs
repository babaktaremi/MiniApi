var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("miniApiDb")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgWeb()
    .WithPgAdmin();

var apiDatabase=db.AddDatabase("miniDb","miniDb");

builder.AddProject<Projects.MiniApi>("miniApi")
    .WithExternalHttpEndpoints()
    .WithReference(apiDatabase)
    .WaitFor(apiDatabase)
    .WithHttpHealthCheck("/healthcheck");

builder.Build().Run();