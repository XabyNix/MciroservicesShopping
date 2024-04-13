using System;
using System.Collections.Generic;
using System.Linq;
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

    public IEnumerable<Item> GetCatalog()
    {
        var items = _context.CatalogItems.AsEnumerable<Item>();
        return items;
    }

    public Item? GetCatalogItem(System.Guid id)
    {
        var itemItem = _context.CatalogItems.FirstOrDefault(c => c.Id == id);
        return itemItem;
    }

    public IEnumerable<Item> GetItemsFromIds(IEnumerable<Guid> ids)
    {
        var items = _context.CatalogItems.Where(i => ids.Contains(i.Id)).AsEnumerable();
        return items;
    }
    public Item? GetCatalogItemByName(string name)
    {
        var itemByName = _context.CatalogItems
        .FirstOrDefault
        (c => c.ProductName != null &&
        c.ProductName.ToUpper().Equals(name.ToUpper()));
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

    public bool ItemExists(Guid id)
    {
        return _context.CatalogItems.Any(c => c.Id == id);
    }

}
