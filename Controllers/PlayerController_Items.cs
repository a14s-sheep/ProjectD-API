using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectD_API.Data.Messages;
using ProjectD_API.Data.Models;
using System.Collections.Generic;

namespace ProjectD_API.Controllers
{
    public partial class PlayerController
    {
        private async Task FillEquipItems(string playerId, List<PlayerItem> list)
        {
            await DeletePlayerItems(playerId, 0);
            await CreatePlayerItems(playerId, 0, list);
        }

        private async Task FillInventoryItems(string playerId, List<PlayerItem> list)
        {
            await DeletePlayerItems(playerId, 1);
            await CreatePlayerItems(playerId, 1, list);
        }


        private async Task CreatePlayerItems(string playerId, byte inventoryType, List<PlayerItem> list)
        {
            Console.WriteLine($"LOGGING: Adding {list.Count} items");
            if (list == null || list.Count == 0) return;

            foreach (var item in list)
            {
                item.Id = Guid.NewGuid().ToString();
                item.PlayerId = playerId;
                item.InventoryType = inventoryType;
            }
            _context.PlayerItems.AddRange(list);
        }

        private async Task DeletePlayerItems(string playerId, byte inventoryType)
        {
            var items = await _context.PlayerItems.Where(pi => pi.PlayerId == playerId && pi.InventoryType == inventoryType).ToArrayAsync();
            Console.WriteLine($"LOGGING: Deleting {items.Count()} items");
            if (items.Any()) _context.PlayerItems.RemoveRange(items);
        }


        [Authorize]
        [HttpPost("item/get-items")]
        public async Task<IActionResult> GetPlayerItems([FromBody] string playerId)
        {
            if (playerId == null || string.IsNullOrEmpty(playerId)) return BadRequest("Request body is not valid");

            List<PlayerItem> playerItems = await _context.PlayerItems.Where(i => i.PlayerId == playerId).ToListAsync();
            if (playerItems == null || playerItems.Count == 0) return BadRequest("Player's items is not found!");

            return Ok(playerItems);
        }

        [Authorize]
        [HttpPost("item/add-items")]
        public async Task<IActionResult> PlayerAddItems([FromBody] PlayerAddItemsRequest request)
        {
            if (request.PlayerId == null || string.IsNullOrEmpty(request.PlayerId))
                return BadRequest("Player ID is not valid");

            if (request.Items != null && request.Items.Count > 0)
            {
                foreach (var item in request.Items)
                {
                    PlayerItem playerItem = _mapper.Map<PlayerItem>(item);

                    playerItem.Id = Guid.NewGuid().ToString();
                    playerItem.PlayerId = request.PlayerId;

                    _context.PlayerItems.Add(playerItem);
                }
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("item/update-item")]
        public async Task<IActionResult> PlayerUpdateItem([FromBody] PlayerUpdateItemsRequest request)
        {
            if (request.PlayerId == null || string.IsNullOrEmpty(request.PlayerId))
                return BadRequest("Player ID is not valid");

            if (request.Items != null && request.Items.Count > 0)
            {
                foreach (var item in request.Items)
                {
                    var playerItem = await _context.PlayerItems.FirstOrDefaultAsync(x => x.PlayerId == request.PlayerId && x.Id == item.Id && x.DataId == item.DataId);
                    if (playerItem != null)
                        _mapper.Map(item, playerItem);
                    else continue;
                }
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("item/remove-item")]
        public async Task<IActionResult> PlayerRemoveItem([FromBody] PlayerRemoveItemRequest request)
        {
            if (request.PlayerId == null || string.IsNullOrEmpty(request.PlayerId))
                return BadRequest("Player ID is not valid");

            if (request.Items != null && request.Items.Count > 0)
            {
                foreach (var item in request.Items)
                {
                    var playerItem = await _context.PlayerItems.FirstOrDefaultAsync(x => x.PlayerId == request.PlayerId && x.Id == item.Id && x.DataId == item.DataId && x.SlotIndex == item.SlotIndex && x.InventoryType == item.InventoryType);
                    if (playerItem != null) _context.Remove(playerItem);
                    else continue;
                }
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        //[Authorize]
        //[HttpPost("item/equip-item")]
        //public async Task<IActionResult> PlayerEquipItem()
        //{

        //}

        //[Authorize]
        //[HttpPost("item/unequip-item")]
        //public async Task<IActionResult> PlayerUnequipItem()
        //{

        //}


    }
}
