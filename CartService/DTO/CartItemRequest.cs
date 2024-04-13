using System.ComponentModel.DataAnnotations;

namespace CartService;

public class CartItemRequest
{
    [Required]
    public Guid ItemId { get; set; }
    [Required]
    public Guid CartId { get; set; }
    [Required]
    public int Quantity { get; set; } = 1;
}
