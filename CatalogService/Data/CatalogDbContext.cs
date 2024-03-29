using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data;

public class CatalogDbContext : DbContext
{
    public DbSet<Item> CatalogItems { get; set; }
    public CatalogDbContext(DbContextOptions options) : base(options)
    {

    }
}
