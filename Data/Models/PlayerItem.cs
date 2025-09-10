using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectD_API.Data.Models
{
    public class PlayerItem
    {
        [Key]
        public required string Id { get; set; }
        public required string PlayerId { get; set; }

        public int DataId { get; set; }
        public int Amount { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public byte Rarity { get; set; }
        public float Durability { get; set; }
        public int InventoryIndex { get; set; }
        public byte InventoryType { get; set; } // 0 = Equip, 1 = Inventory, 2 = Storage

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
