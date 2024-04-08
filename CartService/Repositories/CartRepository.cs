using System.Text.Json;
using CartService.Models;
using CartService.Repositories.Interfaces;

namespace CartService.Repositories;

public class CartRepository : ICartRepository
{
    private readonly CartDbContext _context;
    private readonly HttpClient _client;
    public CartRepository(CartDbContext context, IHttpClientFactory client)
    {
        _client = client.CreateClient();
        _context = context;
    }

    public IEnumerable<Cart> GetCarts()
    {
        var carts = _context.Carts.AsEnumerable();
        return carts;
    }

    public Cart GetCart(Guid CartId)
    {
        var cartFound = _context.Carts.FirstOrDefault(c => c.Id == CartId)!;
        return cartFound;
    }

    public Cart GetCartByUser(Guid UserId)
    {
        var userCart = _context.Carts.FirstOrDefault(c => c.CreatedBy == UserId)!;
        return userCart;
    }

    public void AddCart(Cart Cart)
    {
        _context.Carts.Add(Cart);
        _context.SaveChanges();
    }

    public void UpdateCart(Cart Cart)
    {
        _context.Carts.Update(Cart);
        _context.SaveChanges();
    }

    public bool CartExists(Guid id)
    {
        return _context.Carts.Any(c => c.Id == id);
    }

    public async Task AddItemToCart(CartItem item)
    {
        /*var existingItem = _context.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
            _context.Items.Update(existingItem);
            _context.SaveChanges();
            return;
        }*/
        await _context.Items.AddAsync(item);
        _context.SaveChanges();

    }
    public IEnumerable<CartItem> GetItemFromCart(Guid cartId)
    {
        return _context.Items.Where(i => i.CartId == cartId).AsEnumerable();
    }

    public CartItem RemoveItemFromCart(Guid itemId, Guid cartId)
    {
        var itemToRemove = _context.Items.FirstOrDefault(i => i.Id == itemId && i.CartId == cartId);

        if (itemToRemove == null) return null;
        _context.Items.Remove(itemToRemove);
        return itemToRemove;

    }
}