namespace MusicApp.Music.API.DTOs
{
    public class MusicUploadRequestDto
    {
        public string Name { get; set; }
        public IFormFile MusicFile { get; set; }
        public IFormFile? PhotoFile { get; set; }
    }
}
