namespace MusicApp.Music.API.Services
{
    public interface IFileService
    {
        Task<string?> UploadFile(IFormFile file, string folderName);
        Task<bool> DeleteFile(string filePath);
    }
}
