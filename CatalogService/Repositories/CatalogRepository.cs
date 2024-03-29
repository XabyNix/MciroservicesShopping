using CatalogService.Data;
using CatalogService.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CatalogService;

public class CatalogRepository : ICatalogRepository
{
    private readonly CatalogDbContext _context;

    public CatalogRepository(CatalogDbContext context)
    {
        _context = context;
    }

    IEnumerable<Item> ICatalogRepository.GetCatalog()
    {
        var items = _context.CatalogItems.AsEnumerable<Item>();
        return items;
    }

    Item? ICatalogRepository.GetCatalogItem(System.Guid id)
    {
        var itemItem = _context.CatalogItems.FirstOrDefault(c => c.Id == id);
        return itemItem;
    }

    Item? ICatalogRepository.GetCatalogItemByName(string name)
    {
        var itemByName = _context.CatalogItems
        .FirstOrDefault
        (c => c.ProductName != null &&
        c.ProductName.ToUpper().Equals(name.ToUpper()));
        return itemByName;
    }

    void ICatalogRepository.AddItem(Item item)
    {
        _context.CatalogItems.Add(item);
        _context.SaveChanges();
    }


    void ICatalogRepository.UpdateItem(Item item)
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

    bool ICatalogRepository.ItemExists(Guid id)
    {
        return _context.CatalogItems.Any(c => c.Id == id);
    }

}
