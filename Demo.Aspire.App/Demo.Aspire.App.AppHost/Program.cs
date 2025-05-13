var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var database = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("webhooks");
var frontEnd = builder.AddProject<Projects.Demo_Aspire_App_Web>("webfrontend");


builder.AddProject<Projects.Demo_Aspire_App_ApiService>("apiservice")
    .WithReference(database).WaitFor(database)
    .WithReference(cache)
    .WaitFor(cache)
     .WithExternalHttpEndpoints()
    .WaitFor(frontEnd)
    .WithReference(frontEnd);

builder.Build().Run();
