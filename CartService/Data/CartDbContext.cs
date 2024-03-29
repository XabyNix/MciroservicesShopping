using Microsoft.EntityFrameworkCore;
using CartService.Models;

namespace CartService;

public class CartDbContext : DbContext
{
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> Items { get; set; }

    public CartDbContext(DbContextOptions options) : base(options)
    {

    }
}


