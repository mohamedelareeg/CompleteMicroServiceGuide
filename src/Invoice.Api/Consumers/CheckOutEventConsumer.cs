using System;
using System.Threading.Tasks;
using MassTransit;
using Invoice.Api.Dtos;
using Invoice.Api.Services;
using RabbitMQ.Messaging.Events;

namespace Invoice.Api.Consumers
{
    public class CheckOutEventConsumer : IConsumer<CheckOutEvent>
    {
        private readonly IInvoiceService _invoiceService;

        public CheckOutEventConsumer(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        }

        public async Task Consume(ConsumeContext<CheckOutEvent> context)
        {
            var message = context.Message;

            Console.WriteLine($"Received CheckOutEvent - OrderId: {message.OrderId}, TotalAmount: {message.TotalAmount}, CheckoutTime: {message.CheckoutTime}");

            var createInvoiceDto = new CreateInvoiceDto
            {
                InvoiceNumber = $"INV-{Guid.NewGuid().ToString().Substring(0, 8)}",
                InvoiceDate = message.CheckoutTime,
                TotalAmount = (double)message.TotalAmount
            };

            var invoiceId = await _invoiceService.CreateInvoiceAsync(createInvoiceDto);

            Console.WriteLine($"Invoice created with Id: {invoiceId}");
        }
    }
}
