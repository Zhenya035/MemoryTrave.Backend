using Amazon.S3;
using Amazon.S3.Model;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace MemoryTrave.Infrastructure.Cloudflare;

public class CloudStorageService(
    IR2Service clientFactory,
    IOptions<R2Settings> settings) : ICloudStorageService
{
    public async Task<Result<List<string>>> GetPhotoKeysAsync(Guid userId, Guid articleId)
    {
        try
        {
            var client = clientFactory.CreateClient();
            var prefix = $"{userId}/{articleId}/";

            var listResponse = await client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = settings.Value.BucketName,
                Prefix = prefix
            });

            var keys = listResponse.S3Objects
                .Select(o => o.Key)
                .OrderBy(k => k)
                .ToList();

            return Result<List<string>>.Success(keys);
        }
        catch (Exception ex)
        {
            return Result<List<string>>.Failure("Failed to get keys", ErrorCode.InvalidInput);
        }
    }

    public async Task<string> UploadPhotoAsync(string photo, Guid authorId, Guid articleId, int photoNumber)
    {
        var client = clientFactory.CreateClient();
        
        var photoKey = $"{authorId.ToString()}/{articleId.ToString()}/{photoNumber}";
        var photoBytes = Convert.FromBase64String(photo);
        
        using var stream = new MemoryStream(photoBytes);
        
        var request = new PutObjectRequest
        {
            BucketName = settings.Value.BucketName,
            Key = photoKey,
            InputStream = stream,
            ContentType = "application/octet-stream",
            DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true,
        };
        
        await client.PutObjectAsync(request);

        return photoKey;
    }

    public async Task<bool> DeletePhotoAsync(string photoKey)
    {
        try
        {
            var client = clientFactory.CreateClient();
            var key = ExtractKey(photoKey);

            await client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = settings.Value.BucketName,
                Key = key
            });

            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<Result<string>> DownloadPhotoAsync(string photoKey)
    {
        try
        {
            var client = clientFactory.CreateClient();
            var key = ExtractKey(photoKey);

            using var response = await client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = settings.Value.BucketName,
                Key = key
            });

            using var ms = new MemoryStream();
            await response.ResponseStream.CopyToAsync(ms);
        
            var result = Convert.ToBase64String(ms.ToArray());
            return Result<string>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Download failed", ErrorCode.InvalidInput);
        }
    }

    public async Task<Result<List<string>>> DownloadPhotoAsync(Guid userId, Guid articleId)
    {
        var client = clientFactory.CreateClient();
        var prefix = $"{userId}/{articleId}/";

        var listResponse = await client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = settings.Value.BucketName,
            Prefix = prefix
        });

        var photos = new List<string>();

        foreach (var obj in listResponse.S3Objects.OrderBy(o => o.Key))
        {
            using var response = await client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = settings.Value.BucketName,
                Key = obj.Key
            });

            using var ms = new MemoryStream();
            await response.ResponseStream.CopyToAsync(ms);
            photos.Add(Convert.ToBase64String(ms.ToArray()));
        }

        return Result<List<string>>.Success(photos);
    }

    private string ExtractKey(string input)
    {
        var prefix = $"{settings.Value.PublicUrlBase.TrimEnd('/')}/{settings.Value.BucketName}/";
        return input.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? input[prefix.Length..]
            : input;
    }
}