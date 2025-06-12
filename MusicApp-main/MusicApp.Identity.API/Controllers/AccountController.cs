using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Identity.API.Data.Entities;
using MusicApp.Identity.API.DTOs;
using MusicApp.Identity.API.Services;

namespace MusicApp.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager, ITokenService _tokenService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAccount()
        {
            return Ok("from account controller");
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user != null)
            {
                return BadRequest("User already exists");
            }

            var applicationUser = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Username
            };
            var result = await _userManager.CreateAsync(applicationUser, request.Password);
            if (result.Succeeded)
            {
                var roleExist = await _roleManager.RoleExistsAsync("User");
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = "User"
                    });
                }

                var assignResult = await _userManager.AddToRoleAsync(applicationUser, "User");
                if (!assignResult.Succeeded)
                {
                    return BadRequest("Failed to assign role to user");
                }
            }
            else
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is null)
                return NotFound();

            if (await _userManager.CheckPasswordAsync(user, request.Password))
            {
                string token = _tokenService.GenerateLoginToken(user);
                return Ok(new
                {
                    Token = token
                });
            }
            return BadRequest("Wrong password");
        }
    }
}
