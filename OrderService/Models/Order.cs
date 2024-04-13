using System.ComponentModel.DataAnnotations;
using Common.Models;

namespace OrderService.Models;

public record Order()
{
    [Key]
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
    public float Total { get; set; }
    public IEnumerable<OrderItem> Items { get; set; }
    
};

public enum OrderStatus
{
    Pending,
    Confirmed
}

