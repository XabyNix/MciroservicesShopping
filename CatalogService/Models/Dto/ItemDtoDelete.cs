namespace CatalogService.Models.Dto;

public record ItemDtoDelete : ItemDto
{
    public IEnumerable<ItemsDeleteRequest> items { get; set; }
}

public record ItemsDeleteRequest : ItemDto
{
    public Guid Id { get; set; }
}