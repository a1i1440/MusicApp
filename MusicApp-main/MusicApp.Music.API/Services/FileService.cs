
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MusicApp.Music.API.Services
{
    public class FileService : IFileService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        public FileService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"];
        }
        public async Task<bool> DeleteFile(string filePath)
        {
            if (filePath is null)
                return true;
            var blobClient = new BlobContainerClient(_connectionString, _containerName);
            var blob = blobClient.GetBlobClient(filePath.TrimStart('/'));

            var result = await blob.DeleteIfExistsAsync();
            return result.Value;
        }

        public async Task<string?> UploadFile(IFormFile file, string folderName)
        {
            if (file == null) return null;

            var blobClient = new BlobContainerClient(_connectionString, _containerName);
            await blobClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var extension = Path.GetExtension(file.FileName);
            var randomFileName = $"{Guid.NewGuid()}{extension}";
            var blobPath = $"{folderName}/{randomFileName}";
            var blob = blobClient.GetBlobClient(blobPath);

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, overwrite: true);
            }

            return $"/{blobPath}";
        }
    }
}
