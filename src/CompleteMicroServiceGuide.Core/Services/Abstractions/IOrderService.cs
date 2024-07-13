namespace CompleteMicroServiceGuide.Core.Services.Abstractions
{
    public interface IOrderService
    {
        Task CreateOrderAsync(Guid userId);
        Task CancelOrderAsync(Guid orderId, Guid userId);
        Task SubmitOrderAsync(Guid userId);
        Task<List<OrderDto>> GetOrdersAsync(Guid userId);
    }
}
