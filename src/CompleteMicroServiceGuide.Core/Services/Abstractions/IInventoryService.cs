using CompleteMicroServiceGuide.Core.Dtos;

namespace CompleteMicroServiceGuide.Core.Services.Abstractions
{
    public interface IInventoryService
    {
        Task<string> AddProductToInventoryAsync(Guid createdProductId, string name, Guid warehouseId);
        Task<string> RemoveProductFromInventoryAsync(Guid productId, Guid warehouseId);
        Task<string> UpdateProductInfoAsync(Guid productId, Guid warehouseId, string name);
        Task<ProductDetailsDto> GetProductDetailsAsync(Guid productId, Guid warehouseId);
    }

}
