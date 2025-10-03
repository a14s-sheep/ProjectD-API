namespace ProjectD_API.Data.Messages
{
    public class PlayerSkillRequest
    {
        public string PlayerId { get; set; }
        public int DataId { get; set; }
        public int Level { get; set; }
        public double CastTimeEnd { get; set; }
        public double ExecuteTimeEnd { get; set; }
        public double CooldownTimeEnd { get; set; }
        public int CurrentComboIndex { get; set; }
        public double LastUseTime { get; set; }
    }

    public class PlayerSkillAddPointRequest
    {
        public string PlayerId { get; set; }
        public int Point { get; set; }
    }
    public class PlayerSkillRemoveRequest
    {
        public string PlayerId { get; set; }
        public int DataId { get; set; }
    }
}
