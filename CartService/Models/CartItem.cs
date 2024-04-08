using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartService.Models;

public class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid CartId { get; set; }
    
    [Required] 
    [MaxLength(32)]
    public string Name { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public float Price { get; set; }
    
    [MaxLength(256)]
    public string? Description { get; set; }

    public Cart Cart { get; set; } = null!;
}
