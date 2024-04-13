using System.ComponentModel.DataAnnotations;

namespace CartService.Models.Dto;

public class CartRequestDto
{
    [Required]
    public Guid CreatedBy { get; set; }
    
    
}