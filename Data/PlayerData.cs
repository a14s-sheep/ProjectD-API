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
        public int Health { get; set; }
        public int StatPoint { get; set; }

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

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public static class PlayerExtensions
    {
        public static Player GetNewCharacter(this PlayerData dto)
        {
            return new Player()
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Name = dto.Name,
                Level = dto.Level,
                Experience = dto.Experience,
                Health = dto.Health,
                CurrentMap = dto.CurrentMap,
                CurrentPositionX = dto.CurrentPositionX,
                CurrentPositionY = dto.CurrentPositionY,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

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
