namespace MiniApi.Shared.Database.Entities;

public class OrderItemEntity
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid OrderId { get; private set; }
    public int Quantity { get; private set; }

    public OrderItemEntity( Guid productId, int quantity, Guid orderId)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        OrderId = orderId;
    }
}