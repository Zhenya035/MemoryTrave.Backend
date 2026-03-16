using MemoryTrave.Domain.Common;

namespace MemoryTrave.Domain.Interfaces
{
    public interface ICloudStorageService
    {
        Task<Result<string>> DownloadPhotoAsync(string photoKey);
        Task<Result<List<string>>> DownloadPhotoAsync(Guid userId, Guid articleId);
        Task<Result<List<string>>> GetPhotoKeysAsync(Guid userId, Guid articleId);
        
        Task<string> UploadPhotoAsync(string photo, Guid userId, Guid articleId, int photoNumber);
       
        Task<bool> DeletePhotoAsync(string photoKey);
    }
}