using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectD_API.Data.Messages;
using ProjectD_API.Data.Models;
using System.Diagnostics;
using System.Security.Claims;

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

        [Authorize]
        [HttpPost("skill/get-player-skills")]
        public async Task<IActionResult> GetPlayerSkills([FromBody] string playerId)
        {
            List<PlayerSkill> playerSkills = await _context.PlayerSkills.Where(p => p.PlayerId == playerId).ToListAsync();
            if(playerSkills == null || playerSkills.Count == 0) return Ok("Player's skills not found!");


            return Ok(playerSkills);
        }

        [Authorize]
        [HttpPost("skill/add-skill-point")]
        public async Task<IActionResult> AddSkillPoint([FromBody] PlayerSkillAddPointRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.PlayerId))
                return BadRequest("Data is not valid");

            var player = await _context.Players.FirstOrDefaultAsync(x => x.Id == request.PlayerId);
            if (player == null) return NotFound("Player not found");

            player.SkillPoint += request.Point;

            await _context.SaveChangesAsync();

            return Ok("Add player's skill point success");
        }

        [Authorize]
        [HttpPost("skill/add-skill")]
        public async Task<IActionResult> AddSkill([FromBody] PlayerSkillRequest skill)
        {
            /// TODO: Validate player from token (seperate)
            var player = await _context.Players.FirstOrDefaultAsync(x => x.Id == skill.PlayerId);
            if (player == null) return NotFound("Player not found");

            if (await _context.PlayerSkills.FirstOrDefaultAsync(x => x.PlayerId == skill.PlayerId && x.DataId == skill.DataId) != null)
                return Conflict("Player has already unlocked this skill");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                PlayerSkill playerSkill = new();

                playerSkill = _mapper.Map<PlayerSkill>(skill);

                playerSkill.Id = Guid.NewGuid().ToString();
                playerSkill.PlayerId = skill.PlayerId;

                _context.PlayerSkills.Add(playerSkill);

                //if (player.SkillPoint == 0) return BadRequest("Player has no skill point");

                //player.SkillPoint -= 1;
                _context.Players.Update(player);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(playerSkill);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("skill/update-skill")]
        public async Task<IActionResult> UpdateSkill([FromBody] PlayerSkillRequest skill)
        {
            /// TODO: Validate player from token (seperate)
            var player = await _context.Players.FirstOrDefaultAsync(x => x.Id == skill.PlayerId);
            if (player == null) return NotFound("Player not found");

            var playerSkill = await _context.PlayerSkills.FirstOrDefaultAsync(x => x.PlayerId == skill.PlayerId && x.DataId == skill.DataId);
            if (playerSkill == null) return NotFound("Player hasn’t unlocked this skill yet");

            var id = playerSkill.Id;

            playerSkill = _mapper.Map(skill, playerSkill);
            playerSkill.Id = id;
            _context.PlayerSkills.Update(playerSkill);
            await _context.SaveChangesAsync();

            return Ok("Skill updated");
        }

        [Authorize]
        [HttpPost("skill/remove-skill")]
        public async Task<IActionResult> RemoveSkill([FromBody] PlayerSkillRemoveRequest request)
        {
            /// TODO: Validate player from token (seperate)
            if (request == null || string.IsNullOrEmpty(request.PlayerId))
                return BadRequest("Data is not valid");

            var player = await _context.Players.FirstOrDefaultAsync(x => x.Id == request.PlayerId);
            if (player == null) return NotFound("Player not found");

            var playerSkill = await _context.PlayerSkills.FirstOrDefaultAsync(x => x.PlayerId == request.PlayerId && x.DataId == request.DataId);
            if (playerSkill == null) return NotFound("player hasn’t unlocked this skill yet");

            _context.PlayerSkills.Remove(playerSkill);
            await _context.SaveChangesAsync();

            return Ok("Skill removed");
        }
    }
}
