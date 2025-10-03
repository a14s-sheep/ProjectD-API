using ProjectD_API.Data.Models;

namespace ProjectD_API.Data
{
    public class PlayerData
    {
        public string Id { get; set; }
        public string UserId { get; set; }
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

        public List<PlayerQuest> Quests { get; set; }
        public List<PlayerTask> Tasks { get; set; }

        public List<PlayerItem> EquipItems { get; set; }
        public List<PlayerItem> InventoryItems { get; set; }

        public List<PlayerSkill> Skills { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public static class PlayerExtensions
    {
        public static PlayerData GetPlayerData(this Player player)
        {
            return new PlayerData()
            {
                Id = player.Id,
                UserId = player.UserId,
                Name = player.Name,
                Level = player.Level,
                Experience = player.Experience,
                Health = player.Health,
                CurrentMap = player.CurrentMap,
                CurrentPositionX = player.CurrentPositionX,
                CurrentPositionY = player.CurrentPositionY,
                CreatedAt = player.CreatedAt,
                UpdatedAt = player.UpdatedAt
            };
        }
    }
}
