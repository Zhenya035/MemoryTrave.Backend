using Amazon.S3;
using Amazon.S3.Model;
using MemoryTrave.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace MemoryTrave.Infrastructure.Cloudflare;

public class CloudStorageService(
    IR2Service clientFactory,
    IOptions<R2Settings> option) : ICloudStorageService
{
    public async Task<string> UploadPhotoAsync(byte[] photoBytes, string fileName, string userId, bool isEncrypted)
    {
        var client = clientFactory.CreateClient();
        
        var extension = Path.GetExtension(fileName);
        var photoKey = $"user/{userId}/photos/{Guid.NewGuid()}{extension}";
            
        using var stream = new MemoryStream(photoBytes);
            
        var request = new PutObjectRequest
        {
            BucketName = option.Value.BucketName,
            Key = photoKey,
            InputStream = stream,
            //ContentType = isEncrypted ? "application/octet-stream" : GetContentType(fileName),
            DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true
        };

        await client.PutObjectAsync(request);
        
        return $"{option.Value.PublicUrlBase}/{option.Value.BucketName}/{photoKey}";
    }

    public async Task<bool> DeletePhotoAsync(string photoKey)
    {
        var client = clientFactory.CreateClient();
            
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = option.Value.BucketName,
                Key = photoKey,
                /*DisablePayloadSigning = true,
                DisableDefaultChecksumValidation = true*/
            };

            await client.DeleteObjectAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            return false;
        }
    }

    public async Task<byte[]> DownloadPhotoAsync(string photoKey)
    {
        var client = clientFactory.CreateClient();
            
        var request = new GetObjectRequest
        {
            BucketName = option.Value.BucketName,
            Key = photoKey,
            
            /*DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true*/
        };

        using var response = await client.GetObjectAsync(request);
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
            
        return memoryStream.ToArray();
    }
}