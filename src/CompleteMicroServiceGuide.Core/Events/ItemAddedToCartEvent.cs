
// Events
public record ItemAddedToCartEvent(Guid UserId, Guid SelectedProductId, int Quantity, decimal UnitPrice);

