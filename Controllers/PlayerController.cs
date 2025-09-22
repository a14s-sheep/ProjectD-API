using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectD_API.Data;
using ProjectD_API.Data.Messages;
using ProjectD_API.Data.Models;
using ProjectD_API.Helper;

namespace ProjectD_API.Controllers
{
    [Route("api/player")]
    [ApiController]
    public partial class PlayerController : ControllerBase
    {
        private readonly GameDBContext _context;
        private readonly IConfiguration _configuration;

        public PlayerController(GameDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [Authorize]
        [HttpPost("get-players")]
        public async Task<IActionResult> GetCharacterList([FromBody] string userId)
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

        [Authorize]
        [HttpPost("get")]
        public async Task<IActionResult> GetPlayer([FromBody] string playerId)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
            if (player == null) return NotFound(new { message = "Player not found" });

            PlayerData playerData = await GetPlayerDTO(player);
            return Ok(playerData);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerCreateRequest request)
        {
            // Validate request data
            if (string.IsNullOrEmpty(request.Username))
                return BadRequest(new { message = "Request data is invalid" });

            if (await _context.Players.AnyAsync(c => c.Name == request.Username))
                return Conflict(new { message = "Username is already exists" });

            // Create new player data
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            var userId = TokenHelper.GetUserIdFromToken(_configuration, token);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                Player player = new PlayerData().GetNewCharacter();
                player.Id = Guid.NewGuid().ToString();
                player.UserId = userId!;
                player.Name = request.Username;
                player.ClassId = "Default";

                player.Level = 0;
                player.Experience = 0;
                player.Health = 1;
                player.CurrentMap = "Map Default";
                player.CurrentPositionX = 0;
                player.CurrentPositionY = 0;
                player.CreatedAt = DateTime.UtcNow;
                player.UpdatedAt = DateTime.UtcNow;

                _context.Players.Add(player);
                //await FillPlayerData(data);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Player created successfully!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize]
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
                await FillPlayerData(data);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Character updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize]
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

            // Stats
            var stats = await _context.ClassDefaultStats.Where(x => x.ClassId == player.ClassId).ToListAsync();
            foreach (var stat in stats)
            {
                switch (stat.Name)
                {
                    case "MaxHealth":
                        playerDTO.MaxHealth = stat.Value;
                        break;
                    case "HealthRegen":
                        playerDTO.HealthRegen = stat.Value;
                        break;
                    case "Armor":
                        playerDTO.Armor = stat.Value;
                        break;
                    case "Damage":
                        playerDTO.Damage = stat.Value;
                        break;
                    case "AttackSpeed":
                        playerDTO.AttackSpeed = stat.Value;
                        break;
                    case "CritPower":
                        playerDTO.CritPower = stat.Value;
                        break;
                    case "CritChance":
                        playerDTO.CritChance = stat.Value;
                        break;
                    case "ArmorReduction":
                        playerDTO.ArmorReduction = stat.Value;
                        break;
                    case "MoveSpeed":
                        playerDTO.MoveSpeed = stat.Value;
                        break;
                }
            }

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
