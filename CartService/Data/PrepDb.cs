using CartService.Models;

namespace CartService;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {

        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<CartDbContext>());
        }

    }

    private static void SeedData(CartDbContext context)
    {
        if (!context.Carts.Any())
        {
            Console.WriteLine("--> Seeding Data...");

            context.Carts.AddRange(
                new Cart()
                {
                    CreatedBy = new Guid("978dc33a-29c6-4da1-a13b-9f02dfd652c2"),
                    CreatedAt = DateTime.Now,
                },
                new Cart()
                {
                    CreatedBy = new Guid("6f29f308-b735-4cea-8bfc-b6182d58fd4f"),
                    CreatedAt = DateTime.Now,

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
