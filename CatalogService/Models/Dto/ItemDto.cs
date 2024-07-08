namespace CatalogService.Models.Dto;

public record ItemDto
{
    public string ProductName { get; set; } = null!;

    public float Price { get; set; }

    public string? Description { get; set; }

    public int Quantity { get; set; } = 1;
}