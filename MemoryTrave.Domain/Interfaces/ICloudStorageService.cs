using MemoryTrave.Domain.Common;

namespace MemoryTrave.Domain.Interfaces
{
    public interface ICloudStorageService
    {
        Task<Result<string>> DownloadPhotoAsync(string photoKey);
        Task<Result<List<string>>> DownloadPhotoAsync(string author, Guid articleId);
        Task<Result<List<string>>> GetPhotoKeysAsync(string username, Guid articleId);
        
        Task<string> UploadPhotoAsync(string photo, string authorName, Guid articleId, int photoNumber);
       
        Task<bool> DeletePhotoAsync(string photoKey);
    }
}