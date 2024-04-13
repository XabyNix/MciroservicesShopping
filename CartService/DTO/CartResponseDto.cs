using CartService.Models;

namespace CartService.DTO;

public class CartResponseDto
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
}