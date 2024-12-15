namespace MiniApi.Shared.Database.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get;private set; }
    public decimal Price { get;private set; }

    public ProductEntity( string name, decimal price)
    {
        Id=Guid.NewGuid();
        Name = name;
        Price = price;
    }
}