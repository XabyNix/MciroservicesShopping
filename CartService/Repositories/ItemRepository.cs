using CartService.Data;
using CartService.Models;
using CartService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CartService.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly CartDbContext _context;

    public ItemRepository(CartDbContext context)
    {
        _context = context;
    }

    public async Task AddItemToCart(CartItem item)
    {
        var itemExists = await _context.Items.FirstOrDefaultAsync(i => i.Id == item.Id);
        if (itemExists != null)
        {
            itemExists.Quantity += item.Quantity;
            _context.Items.Update(itemExists);
        }
        else
        {
            await _context.Items.AddAsync(item);
        }

        await _context.SaveChangesAsync();
    }

    public CartItem RemoveItemFromCart(Guid itemId, Guid cartId)
    {
        var itemToRemove = _context.Items.FirstOrDefault(i => i.Id == itemId && i.CartId == cartId);

        if (itemToRemove == null) return null;
        _context.Items.Remove(itemToRemove);
        return itemToRemove;
    }
}