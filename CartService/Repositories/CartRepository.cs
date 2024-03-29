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

    public void AddItemToCart(CartItem item)
    {
        _context.Items.Add(item);
        _context.SaveChanges();

    }
    public IEnumerable<CartItem> GetItemFromCart(Guid CartId)
    {
        return _context.Items.Where(i => i.CartId == CartId).AsEnumerable();
    }

    public CartItem RemoveItemFromCart(Guid ItemId, Guid CartId)
    {
        var itemToRemove = _context.Items.FirstOrDefault(i => i.Id == ItemId && i.CartId == CartId);

        if (itemToRemove is not null)
            _context.Items.Remove(itemToRemove);
        return itemToRemove;
    }
}