using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }


    public DbSet<Item> CatalogItems { get; set; }
    public DbSet<ReservedItem> ReservedItems { get; set; }
}