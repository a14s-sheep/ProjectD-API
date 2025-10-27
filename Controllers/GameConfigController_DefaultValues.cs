using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectD_API.Data.Models;
using static ProjectD_API.Data.Messages.ServerConfigMessages;

namespace ProjectD_API.Controllers
{
    public partial class GameConfigController
    {
        [Authorize]
        [HttpPost("default-value/update-stats")]
        public async Task<IActionResult> UpdateClassDefaultStats([FromBody] DefaultStatsUpdateRequest request)
        {
            if (request == null) return BadRequest(new { message = "Request data is invalid" });

            try
            {
                var list = new List<ClassDefaultStat>();
                var listStat = typeof(DefaultStatsUpdateRequest).GetProperties();

                if (listStat.Length == 0) return BadRequest(new { message = "Request data is empty" });

                foreach (var stat in listStat)
                {
                    var value = stat.GetValue(request);
                    list.Add(new ClassDefaultStat
                    {
                        Id = Guid.NewGuid().ToString(),
                        ClassId = "Default",
                        Name = stat.Name,
                        Value = (float)value!
                    });
                }

                if (list.Count == 0) return BadRequest(new { message = "No stat is updated" });

                await _context.ClassDefaultStats.AddRangeAsync(list);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Default stats updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpPost("default-value/add-items")]
        public async Task<IActionResult> AddClassDefaultItems([FromBody] DefaultItemsAddRequest request)
        {
            /// TODO: Need a seperate request message class
            if (string.IsNullOrEmpty(request.ClassId) || request.Items == null || request.Items.Count == 0)
                return BadRequest("Request data is not valid");

            List<ClassDefaultItem> defaultItemList = new();

            foreach (var item in request.Items)
            {
                ClassDefaultItem defaultItem = new ClassDefaultItem();
                defaultItem.Id = Guid.NewGuid().ToString();
                defaultItem.ClassId = "Default";
                defaultItem.DataId = item.DataId;
                defaultItem.Level = item.Level;
                defaultItem.Exp = item.Exp;
                defaultItem.Amount = item.Amount;
                defaultItem.Durability= item.Durability;
                defaultItem.Rarity = item.Rarity;
                defaultItem.SlotIndex = item.SlotIndex;
                defaultItem.InventoryType = item.InventoryType;
            
                defaultItemList.Add(defaultItem);
            }

            await _context.ClassDefaultItems.AddRangeAsync(defaultItemList);
            await _context.SaveChangesAsync();
            return Ok("Default Items Added");
        }

        ///// TODO: Check Id, DataId, Rarity and Amount
        //[Authorize]
        //[HttpPost("default-value/update-item")]
        //public async Task<IActionResult> UpdateClassDefaultItem([])
        //{

        //}

        ///// TODO: Check Id, DataId, Rarity and Amount
        //[Authorize]
        //[HttpPost("default-value/remove-item")]
    }
}
