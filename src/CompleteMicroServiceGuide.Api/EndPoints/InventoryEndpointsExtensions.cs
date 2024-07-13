using CompleteMicroServiceGuide.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CompleteMicroServiceGuide.Api.EndPoints
{
    public static class InventoryEndpointsExtensions
    {
        public static void MapInventoryEndpoints(this WebApplication app)
        {
            // Endpoint to add a new product to inventory
            app.MapPost("/inventory/add/{productId}/{name}/{warehouseId}", async (
                [FromServices] IInventoryService inventoryService,
                [FromRoute] Guid productId,
                [FromRoute] string name,
                [FromRoute] Guid warehouseId) =>
            {
                return await inventoryService.AddProductToInventoryAsync(productId, name, warehouseId);
            });


            // Endpoint to retrieve product details including last quantity and transactions
            app.MapGet("/inventory/product/{productId}/{warehouseId}", async (
                [FromServices] IInventoryService inventoryService,
                [FromRoute] Guid productId,
                [FromRoute] Guid warehouseId) =>
            {
                return await inventoryService.GetProductDetailsAsync(productId, warehouseId);
            });

            // Endpoint to remove a product from inventory
            app.MapDelete("/inventory/remove/{productId}/{warehouseId}", async (
                [FromServices] IInventoryService inventoryService,
                [FromRoute] Guid productId,
                [FromRoute] Guid warehouseId) =>
            {
                return await inventoryService.RemoveProductFromInventoryAsync(productId, warehouseId);
            });

            // Endpoint to update product information (name and price)
            app.MapPut("/inventory/update/{productId}/{name}/{warehouseId}", async (
                [FromServices] IInventoryService inventoryService,
                [FromRoute] Guid productId,
                [FromRoute] string name,
                [FromRoute] Guid warehouseId) =>
            {
                return await inventoryService.UpdateProductInfoAsync(productId, warehouseId, name);
            });

            // Endpoint to add a sale transaction for a product from a specific warehouse
            app.MapPost("/inventory/sale/add/{warehouseId}/{productId}/{quantity}/{price}", async (
                [FromServices] IProductTransactionService inventoryService,
                [FromRoute] Guid warehouseId,
                [FromRoute] Guid productId,
                [FromRoute] int quantity,
                [FromRoute] double price) =>
            {
                return await inventoryService.SaleProductAsync(warehouseId, productId, quantity, price);
            });

            // Endpoint to add a purchase transaction for a product to a specific warehouse
            app.MapPost("/inventory/purchase/add/{warehouseId}/{productId}/{quantity}/{price}", async (
                [FromServices] IProductTransactionService inventoryService,
                [FromRoute] Guid warehouseId,
                [FromRoute] Guid productId,
                [FromRoute] int quantity,
                [FromRoute] double price) =>
            {
                return await inventoryService.PurchaseProductAsync(warehouseId, productId, quantity, price);
            });

            // Endpoint to transfer product between warehouses
            app.MapPost("/inventory/transfer/{sourceWarehouseId}/{targetWarehouseId}/{productId}/{quantity}", async (
                [FromServices] IProductTransactionService inventoryService,
                [FromRoute] Guid sourceWarehouseId,
                [FromRoute] Guid targetWarehouseId,
                [FromRoute] Guid productId,
                [FromRoute] int quantity) =>
            {
                return await inventoryService.TransferProductBetweenWarehousesAsync(sourceWarehouseId, targetWarehouseId, productId, quantity);
            });

            // Endpoint to get all warehouses
            app.MapGet("/warehouses", async ([FromServices] IWarehouseService warehouseService) =>
            {
                var warehouses = await warehouseService.GetAllWarehousesAsync();
                return Results.Ok(warehouses);
            });
        }
    }
}
