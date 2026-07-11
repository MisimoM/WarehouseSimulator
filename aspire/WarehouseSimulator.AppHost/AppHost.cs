var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5432)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Session)
    .WithPgAdmin();

var warehouseDatabase = postgres.AddDatabase("warehouse-db");

builder.AddProject<Projects.WarehouseSimulator_Web>("warehousesimulator-web")
    .WithReference(warehouseDatabase)
    .WaitFor(warehouseDatabase);

builder.Build().Run();
