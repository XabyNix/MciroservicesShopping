using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public string? ProductName { get; set; }
    [Required]
    public float Price { get; set; }
    public string? Description { get; set; }
}
