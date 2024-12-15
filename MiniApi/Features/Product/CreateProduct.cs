using Mediator;
using MiniApi.Shared.Database;
using MiniApi.Shared.Database.Entities;
using MiniApi.Shared.Endpoints;

namespace MiniApi.Features.Product;

public class CreateProduct
{
     public record CreateProductCommand(string Name,decimal Price) : ICommand<CreateProductCommandResult>;

     public record CreateProductCommandResult(Guid Id);


     public class CreateProductCommandHandler(MiniApiDbContext db)
          : ICommandHandler<CreateProductCommand, CreateProductCommandResult>
     {
          public async ValueTask<CreateProductCommandResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
          {
               var product = new ProductEntity(command.Name, command.Price);

               db.Products.Add(product);

               await db.SaveChangesAsync(cancellationToken);

               return new(product.Id);
          }
     }

     private class CreateProductEndpoint : IEndpoint
     {
          public void MapEndpoint(IEndpointRouteBuilder app)
          {
               app.MapPost("/product", async (ISender sender, CreateProductCommand command,CancellationToken cancellationToken) =>
               {
                    var result = await sender.Send(command, cancellationToken);

                    return TypedResults.Created($"/product/{result.Id}");

               }).WithOpenApi().WithName("CreateProduct")
               .WithTags("Product");
          }
     }
}