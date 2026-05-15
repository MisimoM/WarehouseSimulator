var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5432)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Session)
    .WithPgAdmin();

var warehouseDatabase = postgres.AddDatabase("warehouse-db");

var warehouseApi = builder.AddProject<Projects.WarehouseSimulator_Api>("warehousesimulator-api")
    .WithReference(warehouseDatabase)
    .WaitFor(warehouseDatabase);

builder.AddProject<Projects.WarehouseSimulator_Web>("warehousesimulator-web")
    .WithReference(warehouseApi)
    .WaitFor(warehouseApi);

builder.Build().Run();
