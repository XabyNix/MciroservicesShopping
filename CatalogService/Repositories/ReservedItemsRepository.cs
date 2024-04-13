using CatalogService.Data;
using CatalogService.Models;
using CatalogService.Repositories.Interfaces;

namespace CatalogService.Repositories;

public class ReservedItemsRepository : IReservedItemRepository
{
    private readonly CatalogDbContext _context;

    public ReservedItemsRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public void ReserveItems(IEnumerable<ReservedItem> items)
    {
        var reservedItems = _context.ReservedItems.ToList();

        foreach (var item in items)
        {
            var existingItem = reservedItems.FirstOrDefault(i => i.ProductId == item.ProductId);

            //If the item i want to reserve is already present
            //update the quantity otherwise add the item to the table
            if (existingItem == null)
                _context.ReservedItems.Add(item);
            else
                existingItem.QuantityReserved += item.QuantityReserved;
        }

        //_context.SaveChanges();
    }

    public void FreeReservedItem(IEnumerable<ReservedItem> items)
    {
        //for each item from param update the database context
        //to the specific quantity quantity or remove the row if its 0
        var reservedItems = _context.ReservedItems.Where(i => items.Contains(i)).AsEnumerable();

        foreach (var item in items)
        {
            var reservedItem = reservedItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (reservedItem == null) continue;
            if (reservedItem.QuantityReserved > item.QuantityReserved)
                reservedItem.QuantityReserved -= item.QuantityReserved;
            else
                _context.ReservedItems.Remove(reservedItem);
        }

        //_context.SaveChanges();
    }
}