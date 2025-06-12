using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp.Music.API.Data;
using MusicApp.Music.API.Data.Entities;
using MusicApp.Music.API.DTOs;
using MusicApp.Music.API.Services;
using System.Security.Claims;

namespace MusicApp.Music.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MusicsController(ApplicationDbContext _context, IFileService _fileService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetMusics()
        {
            var userId = int.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var list = await _context.Musics.Where(m => m.CreatedByUserId == userId).ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> UploadMusic([FromForm] MusicUploadRequestDto request)
        {

            if (request.MusicFile is null || request.Name is null)
                return BadRequest();
            MusicItem item = new MusicItem();

            if (request.PhotoFile is not null)
                item.PhotoPath = await _fileService.UploadFile(request.PhotoFile, "MusicPhotos");
            if (request.MusicFile is not null)
                item.MusicPath = await _fileService.UploadFile(request.MusicFile, "MusicFiles");

            item.CreatedAt = DateTime.Now;
            item.CreatedByUserId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            item.Name = request.Name;
            await _context.Musics.AddAsync(item);
            await _context.SaveChangesAsync();
            return Ok("Added successfully");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMusic([FromRoute] int id)
        {
            var musicItem = await _context.Musics.FirstOrDefaultAsync(m => m.Id == id);
            if (musicItem is not null)
            {
                await _fileService.DeleteFile(musicItem.PhotoPath);
                await _fileService.DeleteFile(musicItem.MusicPath);
                _context.Musics.Remove(musicItem);
                await _context.SaveChangesAsync();
                return Ok("Deleted successfully");
            }
            return BadRequest();
        }
    }
}
