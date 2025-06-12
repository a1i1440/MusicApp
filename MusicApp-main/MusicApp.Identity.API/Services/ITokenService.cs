using MusicApp.Identity.API.Data.Entities;
using System.Security.Claims;

namespace MusicApp.Identity.API.Services
{
    public interface ITokenService
    {
        ClaimsPrincipal? ValidateToken(string token);
        string GenerateLoginToken(ApplicationUser user);
    }
}
