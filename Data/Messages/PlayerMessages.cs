namespace ProjectD_API.Data.Messages
{
    public class PlayerStat
    {
        public string Name { get; set; }
        public float Value { get; set; }
    }

    public class PlayerCreateRequest
    {
        public string Username { get; set; }
    }

    public class PlayerUpdateRequest
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


    }

}
