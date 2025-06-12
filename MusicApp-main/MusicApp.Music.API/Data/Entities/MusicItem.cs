namespace MusicApp.Music.API.Data.Entities
{
    public class MusicItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoPath { get; set; }
        public string MusicPath { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
