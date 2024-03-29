namespace CartService;

public class CartItemResponse
{
    public Guid ItemId { get; set; }
    public Guid CartId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Name { get; set; }
    public float Price { get; set; }
}