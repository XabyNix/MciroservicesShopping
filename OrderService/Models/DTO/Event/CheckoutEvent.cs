using System.Collections;

namespace OrderService.DTO;

public record CheckoutEvent
{
    public CommandTypes CommandType { get; set; } = CommandTypes.PlaceOrder;
    public int DiscountPerc { get; set; } = 0;
    public float Total { get; set; }
    public IEnumerable<CartItem> OrderItems { get; set; }
}

public enum CommandTypes
{
    PlaceOrder,
    OrderConfirm,
}