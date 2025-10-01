using Microsoft.EntityFrameworkCore;
using ProjectD_API.Data.Models;

namespace ProjectD_API.Controllers
{
    public partial class PlayerController
    {
        private async Task FillSkills(string playerId, List<PlayerSkill> skills)
        {
            await DeleteCharacterSkills(playerId);
            await CreateCharacterSkills(playerId, skills);
        }

        private async Task CreateCharacterSkills(string playerId, List<PlayerSkill> skills)
        {
            if (skills == null || skills.Count == 0) return;

            foreach (var skill in skills)
            {
                skill.Id = Guid.NewGuid().ToString();
                skill.PlayerId = playerId;
            }
            _context.PlayerSkills.AddRange(skills);
        }

        private async Task DeleteCharacterSkills(string playerId)
        {
            var skills = await _context.PlayerSkills.Where(cq => cq.PlayerId == playerId).ToArrayAsync();
            if (skills.Length != 0) _context.RemoveRange(skills);
        }
    }
}
