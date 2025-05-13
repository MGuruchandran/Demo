var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("webhooks");
builder.AddProject<Projects.Webhook_Demo>("webhook-demo")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
