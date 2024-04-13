using CatalogService.Data;
using CatalogService.Models;
using CatalogService.Repositories.Interfaces;

namespace CatalogService.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly CatalogDbContext _context;

    public CatalogRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Item> GetCatalog()
    {
        var items = _context.CatalogItems.AsEnumerable();
        return items;
    }

    public Item GetCatalogItem(Guid id)
    {
        var itemItem = _context.CatalogItems.FirstOrDefault(c => c.Id == id);
        return itemItem!;
    }

    public IEnumerable<Item> GetItemsFromIdList(IEnumerable<Guid> ids)
    {
        var items = _context.CatalogItems.Where(i => ids.Contains(i.Id)).AsEnumerable();
        return items;
    }

    public IEnumerable<Item> GetItemsByName(string name)
    {
        var itemByName = _context.CatalogItems.Where(i => i.ProductName.Equals(name));
        return itemByName;
    }

    public void AddItem(Item item)
    {
        _context.CatalogItems.Add(item);
        _context.SaveChanges();
    }

    public void UpdateItem(Item item)
    {
        var selectedItem = _context.CatalogItems.FirstOrDefault(c => c.Id == item.Id);
        if (selectedItem != null)
        {
            selectedItem.Price = item.Price;
            selectedItem.Description = item.Description;
            selectedItem.ProductName = item.ProductName;
        }

        _context.SaveChanges();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public bool ItemExists(Guid id)
    {
        return _context.CatalogItems.Any(c => c.Id == id);
    }

    public void UpdateManyQuantity(IEnumerable<Item> items)
    {
        var resultItems = _context.CatalogItems.Where(i => items.Contains(i)).AsEnumerable();
        foreach (var item in resultItems)
        {
            var selectedItem = items.FirstOrDefault(i => i.Id == item.Id);
            if (selectedItem != null)
                item.Quantity -= selectedItem.Quantity;
        }

        //_context.SaveChanges();
    }
}