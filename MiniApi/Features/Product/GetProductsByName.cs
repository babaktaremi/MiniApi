using System.Data;
using Dapper;
using Mediator;
using MiniApi.Shared.Endpoints;

namespace MiniApi.Features.Product;

public class GetProductsByName
{
    public record GetProductsByNameQuery(string Name) : IQuery<IList<GetProductsByNameResult>>;

    public record GetProductsByNameResult(Guid ProductId, string ProductName, decimal ProductPrice);

    public class GetProductsByNameHandler(IDbConnection connection)
        : IQueryHandler<GetProductsByNameQuery, IList<GetProductsByNameResult>>
    {
        public async ValueTask<IList<GetProductsByNameResult>> Handle(GetProductsByNameQuery query, CancellationToken cancellationToken)
        {
            var product = await
                connection.QueryAsync<GetProductsByNameResult>(
                    $@"
                    Select 
                    ""Id"" as {nameof(GetProductsByNameResult.ProductId)},
                    ""Name"" as {nameof(GetProductsByNameResult.ProductName)},
                    ""Price"" as {nameof(GetProductsByNameResult.ProductPrice)}
                    FROM prod.Products
                    Where ""Name"" ILike '%' || @name || '%'
                       "
                    , new { name = query.Name });

            
            return product.AsList();
        }
    }

    private class GetProductsByNameEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/product/search", async (string name,ISender sender,CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetProductsByNameQuery(name), cancellationToken);

                return TypedResults.Ok(result);
            }).WithOpenApi()
            .WithTags("product")
            .WithName("GetProductsByName");
        }
    }
}