using MemoryTrave.Application.Dto.Photo;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers;

[Route("photos")]
[Authorize]
public class PhotoController(IWebHostEnvironment env, ICloudStorageService photoService) : BaseController(env)
{
    [HttpGet("{articleId:guid}/download")]
    public async Task<IActionResult> Download(Guid articleId)
    {
        var userId = GetCurrentUserId();
        
        var photos = await photoService.DownloadPhotoAsync(userId, articleId);
        Result<PhotosDto> result;
        
        if(photos.IsSuccess && photos.Data != null)
        {
            var data = new PhotosDto
            {
                Photos = photos.Data
            };
            result = new Result<PhotosDto>(true, data, string.Empty, null);
        }
        else
            result = new Result<PhotosDto>(false, null, photos.Error, photos.ErrorCode);
        
        return HandleResult(result);
    }
    
    [HttpGet("file")]
    public async Task<IActionResult> DownloadByKey([FromQuery] string key)
    {
        if (string.IsNullOrEmpty(key))
            return HandleResult(Result<string>.Failure("Key required", ErrorCode.InvalidInput));

        var photoResult = await photoService.DownloadPhotoAsync(key);
        
        if (!photoResult.IsSuccess)
            return HandleResult(Result<string>.Failure(photoResult.Error, photoResult.ErrorCode));

        return HandleResult(Result<string>.Success(photoResult.Data));
    }

    [HttpPost("{articleId:guid}/upload")]
    public async Task<IActionResult> Upload(Guid articleId, [FromBody] PhotosDto photosDto)
    {
        var userId = GetCurrentUserId();
        var photosUrls = new List<string>();
        
        for (var i = 0; i < photosDto.Photos.Count; i++)
        {
            var photoUrl = await photoService.UploadPhotoAsync(photosDto.Photos[i], userId, articleId, i+1);
            photosUrls.Add(photoUrl);
        }

        var data = new PhotosDto
        {
            Photos = photosUrls
        };

        var result = new Result<PhotosDto>(true, data, string.Empty, null);
        return HandleResult(result);
    }

    [HttpDelete("{articleId:guid}/all")]
    public async Task<IActionResult> DeleteAll(Guid articleId)
    {
        var userId = GetCurrentUserId();
        
        var photosUrls = await photoService.GetPhotoKeysAsync(userId, articleId);
        Result result;
        
        if (photosUrls.IsSuccess && photosUrls.Data != null)
        {
            foreach (var photo in photosUrls.Data)
            {
                await photoService.DeletePhotoAsync(photo);
            }

            result = new Result(true, string.Empty, null);
        }
        else
        {
            result = new Result(false, "Error", ErrorCode.InvalidInput);
        }
        
        return HandleResult(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] PhotosDto photosDto)
    {
        var result = new Result(true, string.Empty, null);
        
        foreach (var photo in photosDto.Photos)
        {
            var response = await photoService.DeletePhotoAsync(photo);
            if(!response)
                result = new Result(false, "Error", ErrorCode.InvalidInput);
        }
        
        return HandleResult(result);
    }
}