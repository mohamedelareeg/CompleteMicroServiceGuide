using Supplier.Api.Dtos;
using Supplier.Api.Models;

namespace Supplier.Api.Services
{
    public interface ISupplierService
    {
        Task<string> CreateSupplierAsync(CreateSupplierDto dto);

        Task<Guid> AddWarehouseToSupplierAsync(string supplierId, CreateWarehouseDto dto);

        Task<Guid> AddProductToWarehouseAsync(string supplierId, Guid warehouseId, CreateProductDto dto);
    }
}
