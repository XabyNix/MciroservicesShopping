using System.Collections;
using Common.Models;

namespace CartService.Models.Dto;
public class CartResponseDto
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable Items { get; set; } = new List<CartItemDto>();
}