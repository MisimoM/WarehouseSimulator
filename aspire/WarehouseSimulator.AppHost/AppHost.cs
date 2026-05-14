var builder = DistributedApplication.CreateBuilder(args);


builder.AddProject<Projects.WarehouseSimulator_Api>("warehousesimulator-api");

builder.AddProject<Projects.WarehouseSimulator_Web>("warehousesimulator-web");

builder.Build().Run();
