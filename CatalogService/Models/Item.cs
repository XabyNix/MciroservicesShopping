using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] [MaxLength(64)] public string ProductName { get; set; } = null!;

    [Required] public float Price { get; set; }

    [MaxLength(256)] public string? Description { get; set; }

    public int Quantity { get; set; } = 1;
}