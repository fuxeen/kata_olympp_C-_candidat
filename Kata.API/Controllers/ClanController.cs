using Kata.Domain.Entities;
using Kata.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kata.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClanController : Controller
    {
        private readonly IClanService _clanService;
        public ClanController(IClanService clanService)
        {
            _clanService = clanService;
        }
        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clan>>> GetAllClans()
        {
            try
            {
                var clans = await _clanService.GetAllClansAsync();
                return Ok(clans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error :" + ex.Message);
            }
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetClanByName(string name)
        {
            try
            {
                var clan = await _clanService.GetClanByNameAsync(name);
                return Ok(clan);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error :" + ex.Message);
            }
        }
        #endregion
        #region POST
        [HttpPost("{nameClan}")]
        public async Task<IActionResult> Add(string nameClan, [FromBody] Army army)
        {
            try
            {
                var result = await _clanService.AddArmyAsync(nameClan, army);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error : " + ex.Message);
            }
        }
        #endregion
        #region PUT
        [HttpPut("{nameClan}/{nameArmy}")]
        public async Task<IActionResult> Update(string nameClan, string nameArmy, [FromBody] Army army)
        {
            try
            {
                var result = await _clanService.UpdateArmyAsync(nameClan, nameArmy, army);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error :" + ex.Message);
            }

        }
        #endregion
        #region DELETE
        [HttpDelete("{nameClan}/{nameArmy}")]
        public async Task<IActionResult> DeleteProduct(string nameClan,string nameArmy)
        {
            try
            {
                await _clanService.RemoveArmyAsync(nameClan, nameArmy);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error :" + ex.Message);
            }
        }
        #endregion

    }
}
