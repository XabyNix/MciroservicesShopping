namespace OrderService.Models.DTO;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
    public float Total { get; set; }
    public IEnumerable<OrderItemDto> Items { get; set; }
}