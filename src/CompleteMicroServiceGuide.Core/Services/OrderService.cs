using CompleteMicroServiceGuide.Core.Services.Abstractions;
using Marten;
using MassTransit;
using RabbitMQ.Messaging.Events;

namespace CompleteMicroServiceGuide.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDocumentSession _session;
        private readonly IBus _bus;
        public OrderService(IDocumentSession session, IBus bus)
        {
            _session = session;
            _bus = bus;
        }

        public async Task CreateOrderAsync(Guid userId)
        {
            var cart = await _session.LoadAsync<Cart>(userId);
            if (cart == null || cart.Items.Count == 0)
            {
                throw new InvalidOperationException("Cart is empty. Cannot create an order without items.");
            }

            var orderId = Guid.NewGuid();
            var orderItems = cart.Items.Select(item => new CartItemDto
            {
                SelectedProductId = item.SelectedProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList();

            _session.Events.StartStream(new OrderCreatedEvent(orderId, userId, orderItems));

            cart.Items.Clear();
            await _session.SaveChangesAsync();

        }

        public async Task CancelOrderAsync(Guid orderId, Guid userId)
        {
            var order = await _session.LoadAsync<Order>(orderId);
            if (order == null || order.UserId != userId)
            {
                throw new InvalidOperationException($"Order {orderId} not found or you do not have permission to cancel this order.");
            }

            _session.Events.Append(orderId, new OrderCancelledEvent(orderId, userId));

            var cart = await _session.LoadAsync<Cart>(userId);
            if (cart == null)
            {
                cart = new Cart { Id = userId };
                _session.Store(cart);
            }

            foreach (var item in order.Items)
            {
                cart.Items.Add(new CartItemDto
                {
                    SelectedProductId = item.SelectedProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            await _session.SaveChangesAsync();

        }

        public async Task SubmitOrderAsync(Guid userId)
        {
            var cart = await _session.LoadAsync<Cart>(userId);
            if (cart == null || cart.Items.Count == 0)
            {
                throw new InvalidOperationException("Cart is empty. Cannot submit an order without items.");
            }

            decimal totalAmount = 0;

            foreach (var cartItem in cart.Items)
            {
                var product = await _session.LoadAsync<Product>(cartItem.SelectedProductId);
                if (product == null || product.Quantity < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Not enough quantity available for product {cartItem.SelectedProductId}. Available: {product?.Quantity ?? 0}");
                }

                decimal subtotal = cartItem.Quantity * cartItem.UnitPrice;
                totalAmount += subtotal;
            }

            var orderId = Guid.NewGuid();
            var orderItems = cart.Items.Select(item => new CartItemDto
            {
                SelectedProductId = item.SelectedProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList();

            _session.Events.StartStream(new OrderCreatedEvent(orderId, userId, orderItems));

            var checkoutEvent = new CheckOutEvent(orderId, totalAmount, DateTime.UtcNow);
            await _bus.Publish(checkoutEvent);

            cart.Items.Clear();
            await _session.SaveChangesAsync();
        }


        public async Task<List<OrderDto>> GetOrdersAsync(Guid userId)
        {
            var orders = await _session.Query<Order>().Where(o => o.UserId == userId).ToListAsync();

            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException("No orders found for this user.");
            }

            var orderDtos = orders.Select(order => new OrderDto
            {
                OrderId = order.Id,
                Items = order.Items,
                Total = order.CalculateTotalPrice(),
                ShippingAddress = order.ShippingAddress,
                ShippingPhoneNumber = order.ShippingPhoneNumber
            }).ToList();

            return orderDtos;
        }
    }

}
