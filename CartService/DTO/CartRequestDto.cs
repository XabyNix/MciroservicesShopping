using System.ComponentModel.DataAnnotations;

namespace CartService.DTO;

public class CartRequestDto
{
    [Required]
    public Guid CreatedBy { get; set; }
    
    
}