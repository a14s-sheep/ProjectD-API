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
    }
}
