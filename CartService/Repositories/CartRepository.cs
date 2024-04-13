using System.Data;
using CartService.Data;
using CartService.Models;
using CartService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CartService.Repositories;

public class CartRepository : ICartRepository
{
    private readonly CartDbContext _context;
    //private readonly HttpClient _client;
    public CartRepository(CartDbContext context, IHttpClientFactory client)
    {
        //_client = client.CreateClient();
        _context = context;
    }

    public IEnumerable<Cart> GetCarts()
    {
        var carts = _context.Carts.AsEnumerable();
        return carts;
    }

    public Cart GetCart(Guid cartId)
    {
        var cartFound = _context.Carts.Include(c => c.Items)
            .FirstOrDefault(c => c.Id == cartId)!;
        return cartFound;
    }

    public Cart GetCartByUser(Guid userId)
    {
        var userCart = _context.Carts.FirstOrDefault(c => c.CreatedBy == userId)!;
        return userCart;
    }

    public void AddCart(Cart cart)
    {

        if (_context.Carts.Any(c => c.CreatedBy == cart.CreatedBy))
        {
            throw new DuplicateNameException();
        }
        _context.Carts.Add(cart);
        _context.SaveChanges();
        
    }

    public bool CartExists(Guid id)
    {
        return _context.Carts.Any(c => c.Id == id);
    }

    
}