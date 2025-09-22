namespace ProjectD_API.Data.Messages
{
    public partial class ServerConfigMessages
    {
        public class DefaultStatsUpdateRequest()
        {
            public float MaxHealth { get; set; }
            public float HealthRegen { get; set; }
            public float MoveSpeed { get; set; }
            public float AttackSpeed { get; set; }
            public float Damage { get; set; }
            public float CritChance { get; set; }
            public float CritPower { get; set; }
            public float ArmorReduction { get; set; }
            public float Armor { get; set; }
        }
    }
}
