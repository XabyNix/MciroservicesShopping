using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartService.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    }
}