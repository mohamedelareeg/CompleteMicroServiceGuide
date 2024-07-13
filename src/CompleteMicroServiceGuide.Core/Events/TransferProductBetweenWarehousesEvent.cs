namespace CompleteMicroServiceGuide.Core.Events
{
    public record TransferProductBetweenWarehousesEvent(Guid Id, Guid SourceWarehouseId, Guid TargetWarehouseId, Guid ProductId, int Quantity);
}
