using CartService.InternalComunication;
using CartService.Models;

namespace CartService.DTO.InternalComunication;

public record PlaceOrderEvent
{
    public CommandTypes CommandType { get; set; } = CommandTypes.PlaceOrder;
    public int DiscountPerc { get; set; } = 0;
    public float Total { get; set; }
    public IEnumerable<CartItemResponse> OrderItems { get; set; }
}
