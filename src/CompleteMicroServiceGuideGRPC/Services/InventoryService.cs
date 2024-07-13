using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using CompleteMicroServiceGuide.Core.Services.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EventSourcingGRPC.Protos;

namespace CompleteMicroServiceGuideGRPC.Services
{
    public class InventoryService : InventoryProtoService.InventoryProtoServiceBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly IProductTransactionService _productTransactionService;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IInventoryService inventoryService, ILogger<InventoryService> logger, IProductTransactionService productTransactionService)
        {
            _inventoryService = inventoryService;
            _logger = logger;
            _productTransactionService = productTransactionService;
        }

        public override async Task<AddProductToInventoryResponse> AddProductToInventory(AddProductToInventoryRequest request, ServerCallContext context)
        {
            var result = await _inventoryService.AddProductToInventoryAsync(Guid.Parse(request.ProductId), request.Name, Guid.Parse(request.WarehouseId));
            return new AddProductToInventoryResponse
            {
                Message = result
            };
        }

        public override async Task<GetProductDetailsResponse> GetProductDetails(GetProductDetailsRequest request, ServerCallContext context)
        {
            var product = await _inventoryService.GetProductDetailsAsync(Guid.Parse(request.ProductId), Guid.Parse(request.WarehouseId));
            var productInfo = new ProductInfo
            {
                Name = product.Name,
                CurrentQuantity = product.CurrentQuantity,
            };
            foreach (var transaction in product.Transactions)
            {
                var transactionDto = new ProductTransactionDto
                {
                    Id = transaction.Id.ToString(),
                    QuantityChanged = transaction.QuantityChanged,
                    LastPrice = transaction.LastPrice,
                    TransactionType = transaction.TransactionType
                };
                productInfo.Transactions.Add(transactionDto);
            }
           
            return new GetProductDetailsResponse
            {
                ProductInfo = productInfo
            };
        }

        public override async Task<RemoveProductFromInventoryResponse> RemoveProductFromInventory(RemoveProductFromInventoryRequest request, ServerCallContext context)
        {
            var result = await _inventoryService.RemoveProductFromInventoryAsync(Guid.Parse(request.ProductId), Guid.Parse(request.WarehouseId));
            return new RemoveProductFromInventoryResponse
            {
                Message = result
            };
        }

        public override async Task<UpdateProductInfoResponse> UpdateProductInfo(UpdateProductInfoRequest request, ServerCallContext context)
        {
            var result = await _inventoryService.UpdateProductInfoAsync(Guid.Parse(request.ProductId), Guid.Parse(request.WarehouseId), request.Name);
            return new UpdateProductInfoResponse
            {
                Message = result
            };
        }

        public override async Task<TransactionResponse> SaleProduct(SaleProductRequest request, ServerCallContext context)
        {
            var result = await _productTransactionService.SaleProductAsync(Guid.Parse(request.ProductId), Guid.Parse(request.WarehouseId), request.Quantity, request.Price);
            return new TransactionResponse
            {
                Message = result
            };
        }

        public override async Task<TransactionResponse> PurchaseProduct(PurchaseProductRequest request, ServerCallContext context)
        {
            var result = await _productTransactionService.PurchaseProductAsync(Guid.Parse(request.ProductId), Guid.Parse(request.WarehouseId), request.Quantity, request.Price);
            return new TransactionResponse
            {
                Message = result
            };
        }

        public override async Task<TransactionResponse> TransferProductBetweenWarehouses(TransferProductRequest request, ServerCallContext context)
        {
            var result = await _productTransactionService.TransferProductBetweenWarehousesAsync(Guid.Parse(request.SourceWarehouseId), Guid.Parse(request.TargetWarehouseId), Guid.Parse(request.ProductId), request.Quantity);
            return new TransactionResponse
            {
                Message = result
            };
        }
    }
}
