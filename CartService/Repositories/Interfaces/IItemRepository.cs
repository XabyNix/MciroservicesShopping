using CartService.Models;

namespace CartService.Repositories.Interfaces;

public interface IItemRepository
{
    public Task AddItemToCart(CartItem itemId);
    public CartItem RemoveItemFromCart(Guid itemId, Guid cartId);
}