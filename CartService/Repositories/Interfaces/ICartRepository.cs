using CartService.Models;

namespace CartService.Repositories.Interfaces;

public interface ICartRepository
{
    public IEnumerable<Cart> GetCarts();
    public Cart GetCart(Guid CartId);
    public Cart GetCartByUser(Guid UserId);
    public void AddCart(Cart cart);
    public void AddItemToCart(CartItem ItemId);
    public IEnumerable<CartItem> GetItemFromCart(Guid CartId);
    public CartItem RemoveItemFromCart(Guid ItemId, Guid CartId);
    public void UpdateCart(Cart cart);
    public bool CartExists(Guid id);

}
