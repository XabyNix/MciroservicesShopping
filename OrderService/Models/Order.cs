using System.ComponentModel.DataAnnotations;
using OrderService.DTO;

namespace OrderService.Models;

public record Order()
{
    [Key]
    public Guid OrderId { get; set; }
    public int DiscountPerc { get; set; }
    public OrderStatus status { get; set; }
    public float Total { get; set; }
    public IEnumerable<CartItem> Items { get; set; }
    
    public enum OrderStatus
    {
        Pending,
        Confirmed
    }
};

