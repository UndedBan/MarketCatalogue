using MarketCatalogue.Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketCatalogue.Commerce.Infrastructure.Data;

public class CommerceDbContext : DbContext
{
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options)
    { }
}
