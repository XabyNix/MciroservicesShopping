using CatalogService.Models;

namespace CatalogService;

public interface ICatalogRepository
{
    public IEnumerable<Item> GetCatalog();
    public Item? GetCatalogItem(Guid id);
    public Item? GetCatalogItemByName(string name);
    //public IEnumerable<Item> GetItemFromCart(Guid CartId);
    public void AddItem(Item item);
    public void UpdateItem(Item item);
    public bool ItemExists(Guid id);
}
