using System.Runtime.CompilerServices;
using OrderService.Models;

namespace OrderService.Database;

public static class MyDatabase
{
    public static ICollection<Order> OrderDatabase { get; set; }

    public static void Push(Order order)
    {
        order.OrderId = new Guid();
        /*if (!OrderDatabase.Select(i => i.OrderId).Contains(order.OrderId))
        {
            Console.WriteLine("--> Added an item to Database");
        }*/
        OrderDatabase.Add(order);
        
        Console.WriteLine("Elemento già presente nel database");
        
    }

    public static void Remove(Order order)
    {
        if (OrderDatabase.Remove(order))
        {
            Console.WriteLine("Elemento rimosso dal database correttamente.");
        };
        Console.WriteLine("Non è stato possibile rimuovere l'elemento");
    }

    public static Order Get(Order order)
    {
        var item = OrderDatabase.FirstOrDefault(o => o.OrderId == order.OrderId);
        return item ?? null!;
    }
}