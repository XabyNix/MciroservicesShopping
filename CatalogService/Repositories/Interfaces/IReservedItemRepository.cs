using CatalogService.Models;

namespace CatalogService.Repositories.Interfaces;

public interface IReservedItemRepository
{
    void ReserveItems(IEnumerable<ReservedItem> items);
    void FreeReservedItem(IEnumerable<ReservedItem> items);
}