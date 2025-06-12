namespace MusicApp.Favourites.API.Services
{
    public interface IRedisService
    {
        Task AddToFavouritesAsync(int userId, int musicId);
        Task RemoveFromFavouritesAsync(int userId, int musicId);
        Task<List<int>> GetFavouritesAsync(int userId);
    }
}
