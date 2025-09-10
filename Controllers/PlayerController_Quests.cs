using Microsoft.EntityFrameworkCore;
using ProjectD_API.Data.Models;

namespace ProjectD_API.Controllers
{
    public partial class PlayerController
    {
        private async Task FillQuests(string playerId, List<PlayerQuest> quests, List<PlayerTask> tasks)
        {
            await DeleteCharacterQuests(playerId);
            await CreateCharacterQuests(playerId, quests);

            await DeleteCharacterTasks(playerId);
            await CreateCharacterTasks(playerId, tasks);
        }


        private async Task CreateCharacterQuests(string playerId, List<PlayerQuest> quests)
        {
            if (quests == null || quests.Count == 0) return;

            foreach (var quest in quests)
            {
                quest.Id = Guid.NewGuid().ToString();
                quest.PlayerId = playerId;
            }
            _context.PlayerQuests.AddRange(quests);
        }

        private async Task DeleteCharacterQuests(string playerId)
        {
            var quests = await _context.PlayerQuests.Where(cq => cq.PlayerId== playerId).ToArrayAsync();
            if (quests.Length != 0) _context.RemoveRange(quests);
        }

        private async Task CreateCharacterTasks(string characterId, List<PlayerTask> tasks)
        {
            if (tasks == null || tasks.Count == 0) return;

            foreach (var task in tasks)
            {
                task.Id = Guid.NewGuid().ToString();
                task.PlayerId = characterId;
            }
            _context.PlayerTasks.AddRange(tasks);
        }

        private async Task DeleteCharacterTasks(string playerId)
        {
            var tasks = await _context.PlayerTasks.Where(ct => ct.PlayerId == playerId).ToArrayAsync();
            if (tasks.Length != 0) _context.RemoveRange(tasks);
        }
    }
}
