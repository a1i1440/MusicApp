using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Favourites.API.Services;
using System.Security.Claims;

namespace MusicApp.Favourites.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FavouritesController(IRedisService _redisService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetFavourites()
        {
            var userId = int.Parse(User.FindFirst(c=>c.Type == ClaimTypes.NameIdentifier)?.Value);
            var favouriteMusicIds = await _redisService.GetFavouritesAsync(userId);
            return Ok(favouriteMusicIds);
        }

        [HttpPost]
        public async Task<ActionResult> AddFavourite([FromBody] int musicId)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            await _redisService.AddToFavouritesAsync(userId, musicId);
            return Ok("Added to favourites successfully");
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFavourite([FromBody] int musicId)
        {
            await _redisService.RemoveFromFavouritesAsync(
                int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value),
                musicId);
            return Ok();
        }
    }
}
