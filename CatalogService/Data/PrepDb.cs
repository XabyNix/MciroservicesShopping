

using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {

        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<CatalogDbContext>(), isProduction);
        }

    }

    private static void SeedData(CatalogDbContext context, bool isProduction)
    {
        if (isProduction)
        {
            try
            {
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"==> Problem with Migrations: {e.Message}");
            }
        }


        if (!context.CatalogItems.Any())
        {
            Console.WriteLine("--> Seeding Data...");

            context.CatalogItems.AddRange
            (
                new Item()
                {
                    ProductName = "Nutella",
                    Price = 100
                },
                new Item()
                {
                    ProductName = "Formaggio",
                    Price = 20,
                    Description = "Ottimo formaggio da spalmare",
                },
                new Item()
                {
                    ProductName = "Afftettato",
                    Price = 150,
                    Description = "Buono ma attento che fa ingrassare",
                    Quantity = 2,
                }

            );
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
