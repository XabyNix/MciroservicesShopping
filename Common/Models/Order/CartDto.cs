namespace Common.Models.Order;

public record CartDto()
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<CartItemDto> Items { get; set; }
};