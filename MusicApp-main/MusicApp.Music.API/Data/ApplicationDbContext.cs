using Microsoft.EntityFrameworkCore;
using MusicApp.Music.API.Data.Entities;

namespace MusicApp.Music.API.Data
{
    public class ApplicationDbContext 



    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<MusicItem> Musics { get; set; }
    }
}
