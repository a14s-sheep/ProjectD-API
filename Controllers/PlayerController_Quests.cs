using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectD_API.Data.Messages;
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
            var quests = await _context.PlayerQuests.Where(cq => cq.PlayerId == playerId).ToArrayAsync();
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



        #region QUESTS
        [Authorize]
        [HttpPost("quest/get-quests")]
        public async Task<IActionResult> GetPlayerQuests([FromBody] string playerId)
        {
            PlayerQuestLoadResponse response = new();

            List<PlayerQuest> playerQuests = await _context.PlayerQuests.Where(p => p.PlayerId == playerId).ToListAsync();
            if (playerQuests != null && playerQuests.Count > 0) response.Quests = playerQuests;
            else response.Quests = new();

            List<PlayerTask> playerTasks = await _context.PlayerTasks.Where(p => p.PlayerId == playerId).ToListAsync();
            if (playerTasks != null && playerTasks.Count > 0) response.Tasks = playerTasks;
            else response.Tasks = new();

            return Ok(response);
        }

        [Authorize]
        [HttpPost("quest/active-quest")]
        public async Task<IActionResult> ActiveQuest([FromBody] PlayerQuestRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.PlayerId) || request.Quest == null)
                return BadRequest("Request data is not valid");

            var player = await _context.Players.FirstOrDefaultAsync(x => x.Id == request.PlayerId);

            var quest = await _context.PlayerQuests.FirstOrDefaultAsync(x => x.PlayerId == request.PlayerId && x.Id == request.Quest.Id);

            if (quest == null)
            {
                _context.PlayerQuests.Add(request.Quest);
            }
            else if (quest != null)
            {
                _context.PlayerQuests.Remove(quest);
                _context.PlayerQuests.Add(request.Quest);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("quest/update-quest")]
        public async Task<IActionResult> UpdateQuest()
        {
            return Ok();
        }

        [Authorize]
        [HttpPost("quest/complete-quest")]
        public async Task<IActionResult> CompletedQuest()
        {
            return Ok();
        }

        [Authorize]
        [HttpPost("quest/claim-quest-reward")]
        public async Task<IActionResult> ClaimQuestReward()
        {
            return Ok();
        }
        #endregion


        #region TASKS
        [Authorize]
        [HttpPost("task/get-tasks")]
        public async Task<IActionResult> GetPlayerTasks([FromBody] string playerId)
        {
            List<PlayerTask> playerTasks = await _context.PlayerTasks.Where(p => p.PlayerId == playerId).ToListAsync();
            if (playerTasks == null || playerTasks.Count == 0) return Ok("Player's tasks not found!");
            return Ok(playerTasks);
        }
        #endregion
    }
}
