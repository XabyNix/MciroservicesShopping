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
    public string? Name { get; set; }

    public int Quantity { get; set; } = 1;
    public float Price { get; set; }

    [Required]
    public Cart? Cart { get; set; }
}
