namespace OrderService.Models.DTO;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
    //public int Quantity { get; set; } = 1;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
}