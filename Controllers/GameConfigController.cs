using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectD_API.Data;

namespace ProjectD_API.Controllers
{
    [Route("api/game-config")]
    [ApiController]
    public partial class GameConfigController : ControllerBase
    {
        private readonly GameDBContext _context;

        public GameConfigController(GameDBContext context)
        {
            _context = context;
        }

    }
}
