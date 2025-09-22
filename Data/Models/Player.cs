using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectD_API.Data.Models
{
    public class Player
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string ClassId { get; set; }
        public int DataId { get; set; }
        public string Name { get; set; }

        public int Level { get; set; }
        public int Experience { get; set; }
        public int Health { get; set; }
        public int StatPoint { get; set; }

        public int Power { get; set; }
        public int Agility { get; set; }
        public int Vitality { get; set; }

        public string CurrentMap { get; set; }
        public float CurrentPositionX { get; set; }
        public float CurrentPositionY { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
