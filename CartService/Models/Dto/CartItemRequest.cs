namespace CartService.Models.Dto;

public class CartItemRequest
{
    public Guid ItemId { get; set; }
    public Guid CartId { get; set; }
    public int Quantity { get; set; } = 1;
}
