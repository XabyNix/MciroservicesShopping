using CatalogService.Models;

namespace CatalogService.Repositories.Interfaces;

public interface ICatalogRepository
{
    IEnumerable<Item> GetCatalog();
    Item GetCatalogItem(Guid id);
    IEnumerable<Item> GetItemsByName(string name);
    IEnumerable<Item> GetItemsFromIdList(IEnumerable<Guid> ids);
    void AddItem(Item item);
    void UpdateItem(Item item);
    void UpdateManyQuantity(IEnumerable<Item> items);
    bool ItemExists(Guid id);
    Task SaveChanges();
}