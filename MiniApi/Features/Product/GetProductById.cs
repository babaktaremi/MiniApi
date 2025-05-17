using System.Data;
using Dapper;
using Mediator;
using MiniApi.Shared.Endpoints;

namespace MiniApi.Features.Product;

public class GetProductById
{
    public record GetProductByIdRequest(Guid Id) : IQuery<GetProductByIdResult?>;

    public record GetProductByIdResult(Guid Id, string Name, decimal Price);

    public class GetProductByIdHandler(IDbConnection connection)
        : IQueryHandler<GetProductByIdRequest, GetProductByIdResult?>
    {
        public async ValueTask<GetProductByIdResult?> Handle(GetProductByIdRequest query, CancellationToken cancellationToken)
        {
            var product = await
                connection.QueryFirstOrDefaultAsync<GetProductByIdResult>(
                    $@"
                    Select 
                    id as {nameof(GetProductByIdResult.Id)},
                    name as {nameof(GetProductByIdResult.Name)},
                    price as {nameof(GetProductByIdResult.Price)}
                    FROM prod.Products
                    Where id = @id
                       "
                    , new { id = query.Id });

            return product;
        }
    }

    private class GetProductByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/product/{id:guid}", async (ISender sender, Guid id) =>
            {
                var result = await sender.Send(new GetProductByIdRequest(id));
                
                return result is null ? Results.NotFound() : Results.Ok(result);
            }).WithOpenApi().WithName("GetProductById")
            .WithTags("Product");
        }
    }
}