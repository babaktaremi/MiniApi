using Microsoft.EntityFrameworkCore;
using MiniApi.Shared.Database.Entities;

namespace MiniApi.Shared.Database;

public class MiniApiDbContext(DbContextOptions<MiniApiDbContext> options):DbContext(options)
{
   
    
    public DbSet<ProductEntity> Products => base.Set<ProductEntity>();
    public DbSet<OrderEntity> Orders => base.Set<OrderEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderEntity>(builder =>
        {
            builder.Property(c => c.OrderName).HasMaxLength(50);

            builder.HasMany(c => c.OrderItems)
                .WithOne()
                .HasForeignKey(c => c.OrderId);

            builder.ToTable("orders", "ord");
        });

        modelBuilder.Entity<OrderItemEntity>(builder =>
        {
            builder.HasIndex(c => c.ProductId);
            builder.HasIndex(c => c.OrderId);
            builder.ToTable("orderitems", "ord");
        });

        modelBuilder.Entity<ProductEntity>(builder =>
        {
            builder.Property(c => c.Name).HasMaxLength(50);
            builder.Property(c => c.Price).HasPrecision(18, 2);
            
            builder.ToTable("products", "prod");
        });
        
        
        
        base.OnModelCreating(modelBuilder);
    }
}