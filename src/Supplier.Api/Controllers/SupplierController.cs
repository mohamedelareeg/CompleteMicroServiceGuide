using Microsoft.AspNetCore.Mvc;
using Supplier.Api.Dtos;
using Supplier.Api.Services;
using System;
using System.Threading.Tasks;

namespace Supplier.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierDto dto)
        {
            var supplierId = await _supplierService.CreateSupplierAsync(dto);
            return Ok(new { SupplierId = supplierId, Message = "Supplier created successfully." });
        }

        [HttpPost("{supplierId}/warehouses")]
        public async Task<IActionResult> AddWarehouseToSupplier(string supplierId, [FromBody] CreateWarehouseDto dto)
        {
            var warehouseId = await _supplierService.AddWarehouseToSupplierAsync(supplierId, dto);
            return Ok(new { WarehouseId = warehouseId, Message = "Warehouse added to supplier." });
        }

        [HttpPost("{supplierId}/warehouses/{warehouseId}/products")]
        public async Task<IActionResult> AddProductToWarehouse(string supplierId, Guid warehouseId, [FromBody] CreateProductDto dto)
        {
            var productId = await _supplierService.AddProductToWarehouseAsync(supplierId, warehouseId, dto);
            return Ok(new { ProductId = productId, Message = "Product added to warehouse." });
        }
    }
}
