using EventSourcingGRPC.Protos;
using MongoDB.Driver;
using Supplier.Api.Dtos;
using Supplier.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Supplier.Api.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IMongoCollection<Suppliers> _suppliersCollection;
        private readonly InventoryProtoService.InventoryProtoServiceClient _inventoryGRPC;

        public SupplierService(InventoryProtoService.InventoryProtoServiceClient inventoryGRPC)
        {
            var connectionString = "mongodb://mongodb:27017";
            var client = new MongoClient(connectionString);
            var databaseName = "SupplierDB";
            var database = client.GetDatabase(databaseName);
            _suppliersCollection = database.GetCollection<Suppliers>("suppliers");
            _inventoryGRPC = inventoryGRPC;
        }

        public async Task<string> CreateSupplierAsync(CreateSupplierDto dto)
        {
            var supplier = new Suppliers
            {
                Name = dto.Name,
                Warehouses = new List<Warehouses>()
            };
            await _suppliersCollection.InsertOneAsync(supplier);
            return supplier.Id;
        }

        public async Task<Guid> AddWarehouseToSupplierAsync(string supplierId, CreateWarehouseDto dto)
        {
            var warehouseId = Guid.NewGuid();
            var warehouse = new Warehouses
            {
                WarehouseId = warehouseId,
                Location = dto.Location,
                Products = new List<Products>()
            };

            var filter = Builders<Suppliers>.Filter.Eq("_id", supplierId);
            var update = Builders<Suppliers>.Update.Push("Warehouses", warehouse);
            await _suppliersCollection.FindOneAndUpdateAsync(filter, update);
            return warehouseId;
        }

        public async Task<Guid> AddProductToWarehouseAsync(string supplierId, Guid warehouseId, CreateProductDto dto)
        {
            var productId = Guid.NewGuid();
            var product = new Products
            {
                ProductId = productId,
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity
            };

            var filter = Builders<Suppliers>.Filter.And(
                Builders<Suppliers>.Filter.Eq("_id", supplierId),
                Builders<Suppliers>.Filter.ElemMatch("Warehouses", Builders<Warehouses>.Filter.Eq("WarehouseId", warehouseId))
            );

            var update = Builders<Suppliers>.Update.Push("Warehouses.$.Products", product);
            await _suppliersCollection.FindOneAndUpdateAsync(filter, update);

            // Example of calling external service (gRPC)
            var request = new AddProductToInventoryRequest { ProductId = productId.ToString(), Name = product.Name, WarehouseId = warehouseId.ToString() };
            var response = await _inventoryGRPC.AddProductToInventoryAsync(request);

            return productId;
        }
    }
}
