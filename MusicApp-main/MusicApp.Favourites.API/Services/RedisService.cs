
using StackExchange.Redis;

namespace MusicApp.Favourites.API.Services
{
    public class RedisService : IRedisService
    {

        private readonly IDatabase _database;
        public RedisService(IConnectionMultiplexer _redisMultiplexer)
        {
            _database = _redisMultiplexer.GetDatabase();
        }

        public async Task AddToFavouritesAsync(int userId, int musicId)
        {
            await _database.SetAddAsync($"user:{userId}:favourites", musicId);
        }

        public async Task<List<int>> GetFavouritesAsync(int userId)
        {
            var items = await _database.SetMembersAsync($"user:{userId}:favourites");

            return items.Select(item => (int)item).ToList();
        }

        public Task RemoveFromFavouritesAsync(int userId, int musicId)
        {
            return _database.SetRemoveAsync($"user:{userId}:favourites", musicId);
        }
    }
}
