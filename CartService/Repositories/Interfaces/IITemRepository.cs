using CartService.Models;

namespace CartService.Repositories.Interfaces;

public interface IITemRepository
{
    public Task AddItemToCart(CartItem itemId);
    public CartItem RemoveItemFromCart(Guid itemId, Guid cartId);
}