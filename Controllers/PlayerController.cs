using ProjectD_API.Data;
using ProjectD_API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjectD_API.Controllers
{
    [Route("api/player")]
    [ApiController]
    public partial class PlayerController : ControllerBase
    {
        private readonly GameDBContext _context;

        public PlayerController(GameDBContext context)
        {
            _context = context;
        }


        [HttpPost("get-players")]
        public async Task<IActionResult> GetCharacterList([FromBody]string userId)
        {
            List<PlayerData> playerDTOs = new();
            var players = await _context.Players.Where(p => p.UserId == userId).ToArrayAsync();

            PlayerData tempData;
            foreach (var player in players)
            {
                tempData = await GetPlayerDTO(player);
                playerDTOs.Add(tempData);
            }

            return Ok(playerDTOs);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetPlayer([FromBody] string playerId)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
            if (player == null) return NotFound(new { message = "Player not found" });

            PlayerData playerData = await GetPlayerDTO(player);
            return Ok(playerData);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerData data)
        {
            // Validate request data
            if (string.IsNullOrEmpty(data.UserId) || string.IsNullOrEmpty(data.Name))
                return BadRequest(new { message = "Request data is invalid" });

            if (await _context.Players.AnyAsync(c => c.Name == data.Name))
                return Conflict(new { message = "CharacterName already exists" });

            // Create new player data
            try
            {
                data.Id = Guid.NewGuid().ToString();
                Player player = data.GetNewCharacter();
                _context.Players.Add(player);
                // TODO: Update logic later
                await FillPlayerData(data);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Player created successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdatePlayer([FromBody] PlayerData data)
        {
            // Validate request data
            if (data == null || string.IsNullOrEmpty(data.Id))
                return BadRequest(new { message = "Request data is invalid" });

            try
            {
                var player = await _context.Players.FindAsync(data.Id);
                if (player == null) return BadRequest(new { message = "Player not found" });

                UpdatePlayerData(player, data);
                _context.Players.Update(player);
                // TODO: Update logic later
                await FillPlayerData(data);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Character updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeletePlayer([FromBody] string playerId)
        {
            try
            {
                var character = await _context.Players.FindAsync(playerId);
                if (character == null) return NotFound(new { message = "Character not found" });

                // TODO: Consider soft delete or not

                await _context.SaveChangesAsync();
                return Ok(new { message = "Character not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        private async Task<PlayerData> GetPlayerDTO(Player player)
        {
            PlayerData playerDTO = player.GetPlayerData();

            // Items
            var playerItems = await _context.PlayerItems.Where(pi => pi.PlayerId == player.Id).ToListAsync();
            playerDTO.EquipItems = playerItems.Where(pi => pi.InventoryType == 0).ToList();
            playerDTO.InventoryItems = playerItems.Where(pi => pi.InventoryType == 1).ToList();

            // Quests
            playerDTO.Quests = await _context.PlayerQuests.Where(pq => pq.PlayerId == player.Id).ToListAsync();
            playerDTO.Tasks = await _context.PlayerTasks.Where(pt => pt.PlayerId == player.Id).ToListAsync();

            return playerDTO;
        }

        private void UpdatePlayerData(Player player, PlayerData data)
        {
            player.Id = data.Id;
            player.UserId = data.UserId;
            player.Name = data.Name;
            player.Level = data.Level;
            player.Experience = data.Experience;
            player.Health = data.Health;
            player.CurrentMap = data.CurrentMap;
            player.CurrentPositionX = data.CurrentPositionX;
            player.CurrentPositionY = data.CurrentPositionY;
        }

        private async Task FillPlayerData(PlayerData data)
        {
            await FillEquipItems(data.Id, data.EquipItems);
            await FillInventoryItems(data.Id, data.InventoryItems);
            await FillQuests(data.Id, data.Quests, data.Tasks);
        }
    }
}
