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
        public float Health { get; set; }

        public int StatPoint { get; set; }

        public int Power { get; set; }
        public int Agility { get; set; }
        public int Vitality { get; set; }

        public float MaxHealth { get; set; }
        public float HealthRegen { get; set; }
        public float Armor { get; set; }
        public float Damage { get; set; }
        public float AttackSpeed { get; set; }
        public float CritPower { get; set; }
        public float CritChance { get; set; }
        public float ArmorReduction { get; set; }
        public float MoveSpeed { get; set; }

        public string CurrentMap { get; set; }
        public float CurrentPositionX { get; set; }
        public float CurrentPositionY { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
