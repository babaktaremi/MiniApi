using Mediator;
using MiniApi.Shared.Database;
using MiniApi.Shared.Database.Entities;
using MiniApi.Shared.Endpoints;

namespace MiniApi.Features.Order;

public class CreateOrder
{
    public record CreateOrderRequest(string OrderName):ICommand<CreateOrderResponse>;

    public record CreateOrderResponse(Guid OrderId);
    
    public class CreateOrderHandler(MiniApiDbContext db):ICommandHandler<CreateOrderRequest, CreateOrderResponse>
    {
        public async ValueTask<CreateOrderResponse> Handle(CreateOrderRequest command, CancellationToken cancellationToken)
        {
            var order = OrderEntity.Create(command.OrderName);
            
            db.Orders.Add(order);

            await db.SaveChangesAsync(cancellationToken);

            return new(order.Id);
        }
    }

    private class CreateOrderEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/order",
                async (CreateOrderRequest request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(request, cancellationToken);

                    return TypedResults.Created($"/order/{result.OrderId}");
                })
                .WithTags("order")
                .WithName("CreateOrder");
        }
    }
}