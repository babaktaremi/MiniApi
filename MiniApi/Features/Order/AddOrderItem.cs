using FluentResults;
using Mediator;
using Microsoft.EntityFrameworkCore;
using MiniApi.Shared.Database;
using MiniApi.Shared.Endpoints;


namespace MiniApi.Features.Order;

public class AddOrderItem
{
    public record AddOrderItemCommand(Guid OrderId,Guid ProductId,int Quantity)
        :ICommand<Result<AddOrderItemCommandResult>>;

    public record AddOrderItemCommandResult(Guid OrderId);

    public class AddOrderItemCommandHandler(MiniApiDbContext db)
        : ICommandHandler<AddOrderItemCommand, Result<AddOrderItemCommandResult>>
    {
       
        public async ValueTask<Result<AddOrderItemCommandResult>> Handle(AddOrderItemCommand command, CancellationToken cancellationToken)
        {
            var order=await db.Orders.Include(c=>c.OrderItems).FirstOrDefaultAsync(c=>c.Id.Equals(command.OrderId), cancellationToken: cancellationToken);

            if (order is null)
                return Result.Fail("Order not found");

            order.AddOrderItem(new (command.ProductId,command.Quantity,command.OrderId));

            await db.SaveChangesAsync(cancellationToken);

            return new AddOrderItemCommandResult(order.Id);
        }
    }

    private class AddOrderItemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/order/AddOrderItem",
                async (AddOrderItemCommand command, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(command, cancellationToken);

                    if (result.IsFailed)
                        return Results.BadRequest(result.Errors);

                    return TypedResults.Created($"/order/detail/{result.Value.OrderId}");
                })
                .WithOpenApi()
                .WithTags("order")
                .WithName("AddOrderItem");
        }
    }
}