namespace OrderService.DTO;

public record CartItem
{
    public Guid ProductId { get; set; }
    public Guid CartId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
}