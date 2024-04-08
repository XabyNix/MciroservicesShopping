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
                    Items = new List<CartItem>()
                    {
                        new CartItem()
                        {
                            CartId = new Guid("978dc33a-29c6-4da1-a13b-9f02dfd652c2"),
                            ProductId = new Guid("7f9c8c60-8d73-4e6e-b338-7241197e498d"),
                            Name = "Formaggio",
                            Description = "Lo mangiano i topi",
                            Price = 20,
                            Quantity = 2,
                        },
                        new CartItem()
                        {
                            CartId = new Guid("978dc33a-29c6-4da1-a13b-9f02dfd652c2"),
                            ProductId = new Guid("13befee2-646f-4522-b951-016124e48642"),
                            Name = "Nutella",
                            Price = 100,
                        }
                    }
                },
                new Cart()
                {
                    CreatedBy = new Guid("6f29f308-b735-4cea-8bfc-b6182d58fd4f"),
                    CreatedAt = DateTime.Now,
                    Items = new List<CartItem>()
                    {
                        new CartItem()
                        {
                            CartId = new Guid("6f29f308-b735-4cea-8bfc-b6182d58fd4f"),
                            ProductId = new Guid("a8d5bae6-8320-49e1-bbe5-ef333ec084cc"),
                            Name = "Affettato",
                            Description = "Se ne mangi troppo ti senti male, ma è buono",
                            Price = 150,
                            Quantity = 5,
                        }
                    }
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
