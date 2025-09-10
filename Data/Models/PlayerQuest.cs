using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectD_API.Data.Models
{
    public class PlayerQuest
    {
        [Key]
        public required string Id { get; set; }
        public required string PlayerId { get; set; }

        public required int DataId { get; set; }
        public bool IsTracking { get; set; }
        public bool IsClaimedReward { get; set; }
        public int State { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
