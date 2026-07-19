namespace WarehouseSimulator.Core.Application.Services.StorageLocations;

public interface IStorageLocationService
{
    Task<List<StorageLocationView>> GetStorageLocationsAsync();
}
