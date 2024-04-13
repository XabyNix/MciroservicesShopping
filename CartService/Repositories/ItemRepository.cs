using CartService.Models;
using CartService.Repositories.Interfaces;

namespace CartService.Repositories;

public class ItemRepository : IITemRepository
{
    private readonly CartDbContext _context;

    public ItemRepository(CartDbContext context)
    {
        _context = context;
    }
    public async Task AddItemToCart(CartItem item)
    {
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();
    }
    /*public IEnumerable<CartItem> GetItemFromCart(Guid cartId)
    {
        return _context.Items.Where(i => i.CartId == cartId).AsEnumerable();
    }*/

    public CartItem RemoveItemFromCart(Guid itemId, Guid cartId)
    {
        var itemToRemove = _context.Items.FirstOrDefault(i => i.Id == itemId && i.CartId == cartId);

        if (itemToRemove == null) return null;
        _context.Items.Remove(itemToRemove);
        return itemToRemove;

    }
}