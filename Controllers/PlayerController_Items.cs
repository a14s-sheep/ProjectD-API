using ProjectD_API.Data.Models;
using Microsoft.EntityFrameworkCore;

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
            if (items.Any()) _context.PlayerItems.RemoveRange(items);
        }
    }
}
