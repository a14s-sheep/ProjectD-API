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
        private readonly IMapper _mapper;
        private readonly GameDBContext _context;
        private readonly IConfiguration _configuration;

        public PlayerController(GameDBContext context, IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
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
            if (player == null) return NotFound("Player not found");

            PlayerData playerData = await GetPlayerDTO(player);

            return Ok(playerData);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerCreateRequest request)
        {
            // Validate request data
            if (string.IsNullOrEmpty(request.Username))
                return BadRequest("Request data is invalid");

            if (await _context.Players.AnyAsync(c => c.Name == request.Username))
                return Conflict("Username is already exists");

            // Create new player data
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            var userId = TokenHelper.GetUserIdFromToken(_configuration, token);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var player = new Player();

                player.Id = Guid.NewGuid().ToString();
                player.UserId = userId!;

                player.Name = request.Username;
                player.ClassId = "Default";

                player.Level = 1;
                player.Experience = 0;

                var stats = await _context.ClassDefaultStats.Where(x => x.ClassId == player.ClassId).ToListAsync();
                foreach (var stat in stats)
                {
                    switch (stat.Name)
                    {
                        case "MaxHealth":
                            player.MaxHealth = stat.Value;
                            player.Health = stat.Value;
                            break;
                        case "HealthRegen":
                            player.HealthRegen = stat.Value;
                            break;
                        case "Armor":
                            player.Armor = stat.Value;
                            break;
                        case "Damage":
                            player.Damage = stat.Value;
                            break;
                        case "AttackSpeed":
                            player.AttackSpeed = stat.Value;
                            break;
                        case "CritPower":
                            player.CritPower = stat.Value;
                            break;
                        case "CritChance":
                            player.CritChance = stat.Value;
                            break;
                        case "ArmorReduction":
                            player.ArmorReduction = stat.Value;
                            break;
                        case "MoveSpeed":
                            player.MoveSpeed = stat.Value;
                            break;
                    }
                }

                player.CurrentMap = "Map Default";
                player.CurrentPositionX = 0;
                player.CurrentPositionY = 0;

                player.CreatedAt = DateTime.UtcNow;
                player.UpdatedAt = DateTime.UtcNow;


                _context.Players.Add(player);
                //await FillPlayerData(data);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Player created successfully!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> UpdatePlayer([FromBody] PlayerData data)
        {
            // Validate request data
            if (data == null || string.IsNullOrEmpty(data.Id))
                return BadRequest("Request data is invalid");

            try
            {
                var player = await _context.Players.FindAsync(data.Id);
                if (player == null) return BadRequest("Player not found");

                _mapper.Map(data, player);
                //UpdatePlayerData(player, data);
                _context.Players.Update(player);
                await FillPlayerData(data);

                await _context.SaveChangesAsync();

                return Ok("Character updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("delete")]
        public async Task<IActionResult> DeletePlayer([FromBody] string playerId)
        {
            try
            {
                var character = await _context.Players.FindAsync(playerId);
                if (character == null) return NotFound("Character not found");

                // TODO: Consider soft delete or not

                await _context.SaveChangesAsync();
                return Ok("Character not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        private async Task<PlayerData> GetPlayerDTO(Player player)
        {
            var playerData = _mapper.Map<PlayerData>(player);

            playerData.EquipItems = await _context.PlayerItems.Where(x => (x.PlayerId == player.Id && x.InventoryType == 0)).ToListAsync();
            playerData.InventoryItems = await _context.PlayerItems.Where(x => (x.PlayerId == player.Id && x.InventoryType == 1)).ToListAsync();

            playerData.Tasks = await _context.PlayerTasks.Where(x => x.PlayerId == player.Id).ToListAsync();
            playerData.Quests = await _context.PlayerQuests.Where(x => x.PlayerId == player.Id).ToListAsync();

            playerData.Skills = await _context.PlayerSkills.Where(x => x.PlayerId == player.Id).ToListAsync();

            return playerData;
        }

        private async Task FillPlayerData(PlayerData data)
        {
            await FillEquipItems(data.Id, data.EquipItems);
            await FillInventoryItems(data.Id, data.InventoryItems);
            await FillQuests(data.Id, data.Quests, data.Tasks);
            await FillSkills(data.Id, data.Skills);
        }
    }
}
