var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AspireTesting>("aspiretesting");

builder.Build().Run();
