using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Messaging.Events
{
    public record ProductCreatedEvent : IntegrationEvent
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; }
        public decimal Price { get; init; }

        public ProductCreatedEvent(Guid productId, string productName, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
        }
    }
}
