namespace MiniApi.Shared.Database.Entities;

public class OrderEntity
{
    private readonly List<OrderItemEntity> _orderItems = new();

    public Guid Id { get; private set; }
    public string OrderName { get; private set; }
    public IReadOnlyCollection<OrderItemEntity> OrderItems => _orderItems.AsReadOnly();


    private OrderEntity()
    {
    }

    public static OrderEntity Create(string orderName)
        => new OrderEntity() { OrderName = orderName, Id = Guid.NewGuid() };

    public void AddOrderItem(OrderItemEntity orderItem)
        => _orderItems.Add(orderItem);

    public void RemoveOrderItem(Guid orderItemId)
    {
        var orderItem = _orderItems.FirstOrDefault(o => o.Id == orderItemId);

        if (orderItem is not null)
            _orderItems.Remove(orderItem);
    }
}