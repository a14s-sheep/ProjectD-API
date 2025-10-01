using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectD_API.Data.Models
{
    public class PlayerSkill
    {
        [Key]
        public required string Id { get; set; }
        public required string PlayerId { get; set; }

        public required int DataId { get; set; }
        public int Level { get; set; }
        public double CastTimeEnd { get; set; }
        public double ExecuteTimeEnd { get; set; }
        public double CooldownTimeEnd { get; set; }
        
        public int CurrentComboIndex { get; set; }

        public double LastUseTime { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
