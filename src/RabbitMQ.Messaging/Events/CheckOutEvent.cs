using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Messaging.Events
{
    public record CheckOutEvent : IntegrationEvent
    {
        public Guid OrderId { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime CheckoutTime { get; init; }

        public CheckOutEvent(Guid orderId, decimal totalAmount, DateTime checkoutTime)
        {
            OrderId = orderId;
            TotalAmount = totalAmount;
            CheckoutTime = checkoutTime;
        }
    }
}
