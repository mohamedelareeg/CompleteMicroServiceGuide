using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Supplier.Api.Models
{
    public class Suppliers
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public List<Warehouses>? Warehouses { get; set; }
    }
}
