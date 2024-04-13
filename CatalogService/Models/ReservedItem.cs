using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models;

public class ReservedItem
{
    [Key] public Guid ProductId { get; set; }

    public int QuantityReserved { get; set; }
}