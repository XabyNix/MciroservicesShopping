using OrderService.Models;

namespace OrderService.DTO;

public record OrderDto
{
    public Guid OrderId { get; set; }
    public Order.OrderStatus status { get; set; }
    public int DiscountPerc { get; set; } = 0;
    public float Total { get; set; }
    public IEnumerable<CartItem> Items { get; set; }
    
}