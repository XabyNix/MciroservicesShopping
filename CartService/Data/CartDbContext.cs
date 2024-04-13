using Microsoft.EntityFrameworkCore;
using CartService.Models;

namespace CartService;

public class CartDbContext : DbContext
{
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> Items { get; set; }

    public CartDbContext(DbContextOptions<CartDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Cart>().HasIndex(c => c.CreatedBy).IsUnique();
        base.OnModelCreating(builder);
    }
}


