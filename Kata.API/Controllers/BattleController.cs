using Kata.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kata.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleController : Controller
    {
        private readonly IBattleService _battleService;
        public BattleController(IBattleService battleService)
        {
            _battleService = battleService;
        }
        [HttpPost]
        public async Task<IActionResult> Battle()
        {
            var battleReport = await _battleService.Battle();
            return Ok(battleReport);
        }
    }
}
