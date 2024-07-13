using CompleteMicroServiceGuide.Core.Models;

namespace CompleteMicroServiceGuide.Core.Services.Abstractions
{
    public interface IWarehouseService
    {
        //Task<List<WarehouseProduct>> GetWarehouseProductsAsync(Guid warehouseId);
        Task<List<Warehouse>> GetAllWarehousesAsync();

    }
}
