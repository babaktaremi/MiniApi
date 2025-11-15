using System.Data;
using Dapper;
using Mediator;
using MiniApi.Shared.Endpoints;

namespace MiniApi.Features.Order;

public class GetOrderDetails
{
    public record GetOrderDetailsQuery(Guid OrderId)
        : IQuery<GetOrderDetailsQueryResult>;

    public class GetOrderDetailsQueryResult
    {
        public Guid OrderId { get; init; }
        public List<OrderItems> Items { get; init; } = new();
    }

    public class OrderItems
    {
        public Guid OrderItemId { get; init; } 
        public Guid ProductId { get; init; } 
        public string ProductName { get; init; } 
        public decimal ProductPrice { get; init; } 
        public int Quantity { get; init; } 
        
    }

    public class GetOrderDetailQueryHandler(IDbConnection connection) :
        IQueryHandler<GetOrderDetailsQuery, GetOrderDetailsQueryResult>
    {
        public async ValueTask<GetOrderDetailsQueryResult> Handle(GetOrderDetailsQuery query,
            CancellationToken cancellationToken)
        {
            Dictionary<Guid, GetOrderDetailsQueryResult?> orders = new();

            var result =
                await connection.QueryAsync<GetOrderDetailsQueryResult, OrderItems, GetOrderDetailsQueryResult>(
                    $@"
                         SELECT o.id AS OrderId
                        , oi.id As OrderItemId
                        , oi.productId As ProductId
                        , p.name As ProductName
                        , p.price As ProductPrice
                        , oi.quantity As Quantity 
                        FROM ord.Orders o 
                            INNER JOIN ord.OrderItems oi ON o.id = oi.orderId
                            INNER JOIN prod.Products p ON oi.productid = p.id
                         WHERE o.id = @OrderId
                        "
                    , (queryResult, items) =>
                    {
                        if (orders.TryGetValue(queryResult.OrderId, out var order))
                            queryResult = order;
                        else
                            orders.Add(queryResult.OrderId, queryResult);
                        
                        queryResult.Items.Add(items);

                        return queryResult;
                    }
                    , new { query.OrderId }
                    , splitOn: "OrderItemId");

            return orders[query.OrderId];
        }
    }

    private class GetOrderItemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/order/detail/{orderId:guid}", async (Guid orderId,ISender sender,CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetOrderDetailsQuery(orderId), cancellationToken);
                
                return Results.Ok(result);
            })
            .WithTags("order")
            .WithName("GetOrderDetails");
        }
    }
}