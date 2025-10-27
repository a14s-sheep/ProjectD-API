using ProjectD_API.Data.Models;

namespace ProjectD_API.Data.Messages
{
    public class PlayerQuestLoadResponse
    {
        public List<PlayerQuest> Quests { get; set; }
        public List<PlayerTask> Tasks { get; set; }
    }

    public class PlayerQuestRequest()
    {
        public string PlayerId { get; set; }
        public PlayerQuest Quest { get; set; }
        public List<PlayerTask> Tasks { get; set; }
    }
}
