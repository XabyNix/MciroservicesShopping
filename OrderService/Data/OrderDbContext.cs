using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data;

public class OrderDbContext : DbContext
{

    public DbSet<Order> Type { get; set; }
    
    
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
}