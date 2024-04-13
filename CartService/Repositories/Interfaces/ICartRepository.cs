using CartService.Models;

namespace CartService.Repositories.Interfaces;

public interface ICartRepository
{
    public IEnumerable<Cart> GetCarts();
    public Cart GetCart(Guid CartId);
    public Cart GetCartByUser(Guid UserId);
    public void AddCart(Cart cart);


}
