var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Frontend>("frontend");

builder.AddProject<Projects.WebAPI>("webapi");

builder.Build().Run();
